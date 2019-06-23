
using System.Collections.Generic;
using ICFP2019.Dijkstra;

namespace ICFP2019
{
    public partial class Status
    {
        public void execute(Action a, Wrappy w)
        {
            if (a.IsW) w.Loc += new Point(0, 1);
            if (a.IsA) w.Loc += new Point(-1, 0);
            if (a.IsD) w.Loc += new Point(1, 0);
            if (a.IsS) w.Loc += new Point(0, -1);

            if (a.IsE) w.rotateClockwise();
            if (a.IsQ) w.rotateAntiClockwise();

            if (a.IsB)
            {
                Action.B b = (Action.B) a;
                w.Manips.Add(new Point(b.Item1, b.Item2));
                // TODO: checkare che il punto sia davvero adiacente
            }
            // TODO: gestire F, L e gli altri booster

            updateStatus(w);
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
            map[w.Loc] = Tile.Filled;
            foreach (Point p in w.Manips)
            {
                map[w.absolutePosition(p)] = Tile.Filled;
            }
        }

        public Action computeAction(Goal g)
        {
            if (g.IsGoTo)
            {
                Goal.GoTo goTo = (Goal.GoTo) g;
                int x = goTo.Item1, y = goTo.Item2;
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