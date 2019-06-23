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

        public Solver(Status status0)
        {
            this.status = status0;
            this.solution = new List<List<Action>>();
        }

        public Solver(Status status0, List<List<Action>> wrappiesStartingActions)
        {
            this.status = status0;
            this.wrappiesStartingActions = wrappiesStartingActions;
            this.solution = new List<List<Action>>();
        }

        //public Status CurrentStatus
        //{
        //    get => this.currentStatus;
        //}

        public void Init()
        {
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
                foreach (var w in status.wrappies)
                {
                    Action a = null;
                    if (w.LastAction != Action.C)
                    {
                        if (status.collectedBoosters.Contains(Booster.Cloning)
                            && status.boosters.Exists((kvp) => kvp.Key == Booster.CloningPlatform && kvp.Value.Equals(w.Loc)))
                            a = Action.C;
                        else if (status.collectedBoosters.Contains(Booster.FastWheels))
                            a = Action.F;
                    }
                    w.updateDistMap(status.map, status.goals);
                    Wrappy.PriPath pp = w.BestShortestPath();
                    status.execute(a, w);
                }
            }
        }



    }
}
