using System;
using System.Collections.Generic;

namespace ICFP2019.ShortestTileAlgo
{
    public class ShortestTileAlgo
    {
        private Map<Tile> map;
        public static readonly int UNREACHABLE = Int32.MaxValue;
        public static readonly int MAX_GOALS = 10;

        public ShortestTileAlgo(Map<Tile> map)
        {
            this.map = map;
        }

        private int minDistAround(Map<int> distMap, Point p)
        {
            int min = UNREACHABLE;
            if (p.x > 0 && distMap[p.x - 1, p.y] < min) min = distMap[p.x - 1, p.y];
            if (p.x < map.W && distMap[p.x + 1, p.y] < min) min = distMap[p.x + 1, p.y];
            if (p.y > 0 && distMap[p.x, p.y - 1] < min) min = distMap[p.x, p.y - 1];
            if (p.x < map.H && distMap[p.x, p.y + 1] < min) min = distMap[p.x, p.y + 1];
            return min;
        }

        public Dijkstra.Graph.Result calculateMap(Wrappy w, List<Goal> goals)
        {
            Map<int> distMap = new Map<int>(map.W, map.H);
            int max_goals = Math.Min(MAX_GOALS, goals.Count);

            distMap.fillWith(UNREACHABLE);
            distMap[w.Loc] = 0;
            // sort goals by distance from w and then only keep the first max_goals
            goals.Sort((Goal g1, Goal g2) =>
            {
                return (new Point((Goal.GoTo)g1) - w.Loc).l1norm()
                     - (new Point((Goal.GoTo)g2) - w.Loc).l1norm();
            });
            List<Goal> bestGoals = goals.GetRange(0, max_goals);

            Point p = new Point();
            int R = (w.Loc - new Point((Goal.GoTo)bestGoals[bestGoals.Count - 1])).l1norm();

            // cycle through all radii
            for (int r = 1; r <= R; ++r)
            {
                // cycle through rhombus (i.e. L-1 circle) side length, coordinates
                // are computed as (i, r-i), depends on which side (have l-1 distance
                // = r from wrappy position)
                for (int i = 0; i < r; ++i)
                {
                    // compute offset for each side, sign of
                    // x/y offset based on side is:
                    //
                    //         1     @     0
                    //        -/+  @   @  +/+
                    //           @       @
                    //         @     X     @
                    //           @       @
                    //        -/-  @   @  +/-
                    //         2     @     3
                    //
                    for (int side = 0; side < 4; ++side)
                    {
                        // trick: 0 = 3 (mod 3)
                        int xSign = (side % 3 == 0) ? 1 : -1;
                        int ySign = (side < 2) ? 1 : -1;
                        p.x = w.Loc.x + i * xSign;
                        p.y = w.Loc.y + (r - i) * ySign;
                        if (map.validCoordinate(p) && map[p] != Tile.Obstacle)
                        {
                            distMap[p] = minDistAround(distMap, p) + 1;
                        }
                    }
                }
            }

            List<PriGoal> priGoals = new List<PriGoal>();
            foreach (Goal goal in bestGoals)
            {
                priGoals.Add(new PriGoal {
                    goal = goal,
                    pri = distMap[new Point((Goal.GoTo)goal)]
                });
            }

            return new Dijkstra.Graph.Result { 
                distMap = distMap,
                priGoals = priGoals
            };
        }
    }
}
