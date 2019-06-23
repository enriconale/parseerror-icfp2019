using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFP2019
{
    class Solver
    {
        private Status s;
        private List<List<Action>> wrappiesStartingActions;// TODO ... not used
        public List<List<Action>> solution;

        public Solver(Status startingState) {
            this.s = startingState;
            this.solution = new List<List<Action>>();
        }

        public Solver(Status startingState, List<List<Action>> wrappiesStartingActions)
        {
            this.s = startingState;
            this.wrappiesStartingActions = wrappiesStartingActions;
            this.solution = new List<List<Action>>();
        }

        public void solve() {
            while (!s.isSolved())
            {
                this.solveStep();
            }
        } 

        public void solveStep()
        {
            foreach (var wrappy in s.wrappies)
            {

            }
            

            // 1 AGGIORNAMENTO STATO WRAPPIES {MAPPA DISTANZE} 
            // 2 PER OGNI WRAPPY RICALCOLO OBBIETTIV
            // 3 PER OGNI OBBIETTIVO OTTENERE LE MOSSE
            // 4 AGGREGARE LE MOSSE PER PRIORITA
            // 5 PRENDERE LA MOSSA A PRIORITA'' PIU ALTA
            // 6 ESEGUIRE LA MOSSA
            // 7 AGGIORNARE LA MAPPA GLOBALE


        }


    }
}
