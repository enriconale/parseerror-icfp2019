
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
            if (w.remainingFastWheel > 0)
            {
                w.remainingFastWheel--;
                updateStatus(w);
                Move(a, w);
            }
            if (w.remainingDrill > 0)
            {
                w.remainingDrill--;
                //map[w.Loc.x, w.loc.y] = Tile.Filled; // already done by updatemap
            }

            if (a.IsE) w.rotateClockwise();
            if (a.IsQ) w.rotateAntiClockwise();

            if (a.IsB)
            {
                if (!collectedBoosters.Remove(Booster.Manipulator)) throw new Exception("Missing manipulator boost");
                Action.B b = (Action.B)a;
                w.Manips.Add(new Point(b.Item1, b.Item2));
                // TODO: checkare che il punto sia davvero adiacente
            }
            if (a.IsF)
            {
                if (!collectedBoosters.Remove(Booster.FastWheels)) throw new Exception("Missing fast wheel boost");
                w.remainingFastWheel += 50;
            }
            if (a.IsL)
            {
                if (!collectedBoosters.Remove(Booster.Drill)) throw new Exception("Missing Drill boost");
                w.remainingDrill += 30;

            }
            if (a.IsR)//telepor
            {
                throw new Exception("Teleport not implemented yet");
            }
            if (a.IsC)
            {
                if (!collectedBoosters.Remove(Booster.Cloning)) throw new Exception("Missing Cloning boost");
                if (boosters.Find((kvp) => kvp.Value.Equals(w.Loc)).Key != Booster.CloningPlatform) throw new Exception("Cloning not in a platform");
                wrappies.Add(new Wrappy(w.Loc));

            }
            w.ActionHistory.Add(a);
            updateStatus(w);
        }

        private void Move(Action a, Wrappy w)
        {
            var newLoc = w.Loc + new Point(a);
            if (map.isMovable(newLoc, Tile.Obstacle))  // TOCHECK throwException? // NO, in case of fast wheel
                w.Loc = newLoc;
            else { }
        }


        private void updateStatus(Wrappy w)
        {
            updateMap(w);
            // colleziona il booster se wrappy ci sta sopra
            int i = boosters.FindIndex((kvp) => kvp.Value.Equals(w.Loc));
            if (i >= 0)
            {
                var b = boosters[i];
                collectedBoosters.Add(b.Key);
                if (!(b.Key == Booster.CloningPlatform)) boosters.RemoveAt(i);
            }
            // TODO: usare subito i booster semplici: fastwheel e manip
        }

        private void updateMap(Wrappy w)
        {
            if (map[w.Loc] == Tile.Obstacle && w.remainingDrill <= 0) throw new Exception("Obstacle collision");
            map[w.Loc] = Tile.Filled;
            foreach (Point p in w.Manips)
            {
                Point mp = w.absolutePosition(p);
                if (map.validCoordinate(mp) && map[mp] == Tile.Empty && isVisible(w.Loc, mp))
                {
                    map[mp] = Tile.Filled;
                }
            }
        }

        private bool isVisible(Point p1, Point p2)
        {
            int x0 = Math.Min(p1.x, p2.x),
                y0 = Math.Min(p1.y, p2.y),
                x1 = Math.Max(p1.x, p2.x),
                y1 = Math.Max(p1.y, p2.y);
            Point q1, q2, s;
            if (Math.Abs(x0 - x1) > Math.Abs(y0 - y1))
            {
                for (int x = x0 + 1; x < x1; ++x)
                {
                    q1 = new Point(x, y0);
                    q2 = new Point(x, y1);
                    s = Point.FindIntersection(p1, p2, q1, q2);
                    {
                        if (map.validCoordinate(s) && map[s.x, s.y] == Tile.Obstacle)
                        {
                            return false;
                        }
                    }
                }
            }
            else if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                for (int y = y0 +1; y < y1; ++y)
                {
                    q1 = new Point(x0, y);
                    q2 = new Point(x1, y);
                    s = Point.FindIntersection(p1, p2, q1, q2);
                    s.y -= 1;
                    if (map.validCoordinate(s) && map[s.x, s.y] == Tile.Obstacle)
                    {
                        if (map.validCoordinate(s) && map[s.x, s.y] == Tile.Obstacle)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool isSolved()
        {
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