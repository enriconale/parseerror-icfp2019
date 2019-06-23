using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFP2019
{
    class Solver
    {
        private Status status;
        private List<List<Action>> wrappiesStartingActions;// TODO ... not used
        public List<List<Action>> solution;

        public Solver(Status status0) : this(status0, new List<List<Action>>())
        {

        }

        public Solver(Status status0, List<List<Action>> wrappiesStartingActions)
        {
            this.status = status0;
            this.wrappiesStartingActions = wrappiesStartingActions;
            this.solution = new List<List<Action>>();

            for (int i = 0; i < wrappiesStartingActions.Count; i++)
            {
                var wacs = wrappiesStartingActions[i];
                foreach (var wac in wacs)
                {
                    if (status.wrappies.Count <= i) throw new Exception("Malformed starting action: Using a not spawned wrappy");
                    status.execute(wac, status.wrappies[i]);
                }
            }
        }

        public void Loop()
        {
            while (!status.isSolved())
            {
                StatisticalPrettyPrinter.printStats(status);
                foreach (var w in status.wrappies)
                {
                    DijkstraPrettyPrinter.printDijkstraMap(status, w);
                    Action a = null;
                    if (w.LastAction != Action.C)
                    {
                        if (status.collectedBoosters.Contains(Booster.Cloning)
                            && status.boosters.Exists((kvp) => kvp.Key == Booster.CloningPlatform && kvp.Value.Equals(w.Loc)))
                            a = Action.C;
                        else if (status.collectedBoosters.Contains(Booster.FastWheels))
                            a = Action.F;
                        else if (status.collectedBoosters.Contains(Booster.Manipulator))
                        {
                            w.Manips.Sort((p1, p2) => -(p1.y - p2.y));
                            a = Action.NewB(w.Manips[0].x, w.Manips[0].y + 1);
                        }
                    }
                    if (a == null)
                    {
                        w.updateDistMap(status.map, status.goals);
                        Wrappy.PriPath pp = w.BestShortestPath();
                        Point d = pp.path[0];
                        if (d.x == w.Loc.x - 1 && d.y == w.Loc.y) a = Action.A;
                        if (d.x == w.Loc.x + 1 && d.y == w.Loc.y) a = Action.D;
                        if (d.y == w.Loc.y - 1 && d.x == w.Loc.x) a = Action.S;
                        if (d.y == w.Loc.y + 1 && d.x == w.Loc.x) a = Action.W;
                    }
                    status.execute(a, w);
                    status.CalculateGoals();
                }
            }
        }
    }
}
