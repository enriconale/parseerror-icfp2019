using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFP2019
{
    class Solver
    {
        private Status currentStatus;
        private List<List<Action>> wrappiesStartingActions;// TODO ... not used
        public List<List<Action>> solution;

        public Solver(Status status0)
        {
            this.currentStatus = status0;
            this.solution = new List<List<Action>>();
        }

        public Solver(Status status0, List<List<Action>> wrappiesStartingActions)
        {
            this.currentStatus = status0;
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
                    if (currentStatus.wrappies.Count <= i) throw new Exception("Malformed starting action: Using a not spawned wrappy");
                    currentStatus.execute(wac, currentStatus.wrappies[i]);
                }
            }
        }

        public void Loop()
        {
            while (!currentStatus.isSolved())
            {
                foreach (var w in currentStatus.wrappies)
                {
                    //w.LastAction 
                    w.updateDistMap(currentStatus.map, currentStatus.goals);
                    Wrappy.PriPath pp = w.BestShortestPath();
                    Action a = null;
                    currentStatus.execute(a, w);
                }
            }
        }



    }
}
