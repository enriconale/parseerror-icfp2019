
using System.Collections.Generic;
using ICFP2019.Dijkstra;

namespace ICFP2019
{
    public partial class Status
    {
        public void execute(Action a)
        {
            if (a.IsW) wrappies[0].Loc += new Point(0, 1);
            if (a.IsA) wrappies[0].Loc += new Point(-1, 0);
            if (a.IsD) wrappies[0].Loc += new Point(1, 0);
            if (a.IsS) wrappies[0].Loc += new Point(0, -1);

            if (a.IsE) wrappies[0].rotateClockwise();
            if (a.IsQ) wrappies[0].rotateAntiClockwise();

            if (a.IsB)
            {
                Action.B b = (Action.B) a;
                wrappies[0].Manips.Add(new Point(b.Item1, b.Item2));
                // TODO: checkare che il punto sia davvero adiacente
            }
            // TODO: gestire F, L e gli altri booster

            updateStatus();
        }

        private void updateStatus()
        {
            updateMap();
            // colleziona il booster se wrappy ci sta sopra
            int i = boosters.FindIndex((kvp) => kvp.Value == wrappies[0].Loc);
            if (i >= 0) collectedBoosters.Add(boosters[i].Key);
            // TODO: usare subito i booster semplici: fastwheel e manip
        }

        private void updateMap()
        {
            map[wrappies[0].Loc] = Tile.Filled;
            foreach (Point p in wrappies[0].Manips)
            {
                map[wrappies[0].absolutePosition(p)] = Tile.Filled;
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
    }
}