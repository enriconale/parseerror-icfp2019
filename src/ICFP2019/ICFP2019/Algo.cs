
using System.Collections.Generic;
using ICFP2019.Dijkstra;
using System;

namespace ICFP2019
{
    public partial class Status
    {
        public void execute(Action a, Wrappy w)
        {
            Move(a, w);
            if (w.remainingFastWheel > 0) {
                w.remainingFastWheel--;
                updateStatus(w);
                Move(a, w);
            }
            if (w.remainingDrill > 0) {
                w.remainingDrill--;
                //map[w.Loc.x, w.loc.y] = Tile.Filled; // already done by updatemap
            }

            if (a.IsE) w.rotateClockwise();
            if (a.IsQ) w.rotateAntiClockwise();

            if (a.IsB)
            {
                Action.B b = (Action.B)a;
                w.Manips.Add(new Point(b.Item1, b.Item2));
                // TODO: checkare che il punto sia davvero adiacente
            }
            // TODO: gestire F, L e gli altri booster
            if (a.IsF)
            {
                //Attach fastwheels
                w.remainingFastWheel = 50;
            }
            if (a.IsL)
            {
                //Attach drill
                w.remainingDrill = 30;
            }
            


            w.executeds.Add(a);
            updateStatus(w);
        }

        private static void Move(Action a, Wrappy w)
        {
            if (a.IsW) w.Loc += new Point(0, 1);
            if (a.IsA) w.Loc += new Point(-1, 0);
            if (a.IsD) w.Loc += new Point(1, 0);
            if (a.IsS) w.Loc += new Point(0, -1);
        }

        private void updateStatus(Wrappy w)
        {
            updateMap(w);
            // colleziona il booster se wrappy ci sta sopra
            int i = boosters.FindIndex((kvp) => kvp.Value == w.Loc);
            if (i >= 0) collectedBoosters.Add(boosters[i].Key);
            // TODO: usare subito i booster semplici: fastwheel e manip
        }

        private void updateMap(Wrappy w)
        {
            if (map[w.Loc] == Tile.Obstacle && w.remainingDrill <= 0) throw new Exception("Obstacle collision");
            map[w.Loc] = Tile.Filled;
            foreach (Point p in w.Manips)
            {
                map[w.absolutePosition(p)] = Tile.Filled;
            }
        }

        public List<Action> computeActions(Goal g)
        {
            if (g.IsGoTo)
            {
                Goal.GoTo goTo = (Goal.GoTo) g;
                int x = goTo.Item1, y = goTo.Item2;
                //int dist = DistTo(x, y);
                // CONTINUA...
                
            }
            return null;
        }

        public bool isSolved() {
            for (int h = 0; h < map.H; h++)
            {
                for (int w = 0; w < map.W; w++)
                {
                    if (map[w, h] == Tile.Empty) return false;
                }
            }
            return true;
        }

    }
}