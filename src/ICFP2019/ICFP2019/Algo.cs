
using System.Collections.Generic;
using ICFP2019.Dijkstra;

namespace ICFP2019
{

    public partial class Status
    {
        public void execute(Action a)
        {
            if (a.IsW) wrappy.Loc += new Point(0, 1);
            if (a.IsA) wrappy.Loc += new Point(-1, 0);
            if (a.IsD) wrappy.Loc += new Point(1, 0);
            if (a.IsS) wrappy.Loc += new Point(0, -1);

            if (a.IsE) wrappy.rotateClockwise();
            if (a.IsQ) wrappy.rotateAntiClockwise();

            if (a.IsB)
            {
                Action.B b = (Action.B) a;
                wrappy.Manips.Add(new Point(b.Item1, b.Item2));
                // TODO: checkare che il punto sia davvero adiacente
            }
            // TODO: gestire F, L e gli altri booster

            updateStatus();
        }

        public void updateStatus()
        {
            updateMap();
            // collezione il booster se wrappy ci sta sopra
            int i = boosters.FindIndex((kvp) => kvp.Value == wrappy.Loc);
            if (i >= 0) collectedBoosters.Add(boosters[i].Key);
            // TODO: usare subito i booster semplici: fastwheel e manip
        }

        public void updateMap()
        {
            map[wrappy.Loc] = Tile.Filled;
            foreach (Point p in wrappy.Manips)
            {
                map[wrappy.absolutePosition(p)] = Tile.Filled;
            }
        }

        public List<Action> computeActions(Goal g)
        {
            if (g.IsGoTo)
            {
                Goal.GoTo goTo = (Goal.GoTo) g;
                int x = goTo.Item1, y = goTo.Item2;
                int dist = DistTo(x, y);
                // CONTINUA...
                
            }
            return null;
        }



    }

}