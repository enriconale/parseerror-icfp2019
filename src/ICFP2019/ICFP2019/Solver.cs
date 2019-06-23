﻿using System;
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
            this.solution.Add(new List<Action>());

            for (int i = 0; i < wrappiesStartingActions.Count; i++)
            {
                var wacs = wrappiesStartingActions[i];
                foreach (var wac in wacs)
                {
                    if (status.wrappies.Count <= i) throw new Exception("Malformed starting action: Using a not spawned wrappy");
                    status.execute(wac, status.wrappies[i]);
                    //StupidPrettyPrinter.PrintCurrentSemiFilledMap(status);
                }
            }
            StatisticalPrettyPrinter.printStats(status);
        }

        public void Loop()
        {
            while (!status.isSolved())
            {
                StatisticalPrettyPrinter.printStats(status);
                foreach (var w in status.wrappies)
                {
                    DijkstraPrettyPrinter.printDijkstraMap(status.Map, w);
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
                        w.updateDistMap(status);
                        DijkstraPrettyPrinter.printDijkstraMap(status.Map, w);
                        Wrappy.PriPath pp = w.BestShortestPath();
                        int i = 0;
                        Point d = pp.path[i];
                        a = calculateNextAction(w, a, d);
                        i++;
                        while (this.invertAction(a) == w.LastAction && i < pp.path.Count)
                        {
                            
                            a = calculateNextAction(w, a, pp.path[i]);
                            i++;
                        }
                    }
                    status.execute(a, w);
                    solution[0].Add(a);
                    status.CalculateGoals();
                }
            }
            StatisticalPrettyPrinter.printStats(status);
            DijkstraPrettyPrinter.printDijkstraMap(status.Map, status.wrappies[0]);

        }

        private static Action calculateNextAction(Wrappy w, Action a, Point d)
        {

            if (d.x == w.Loc.x - 1 && d.y == w.Loc.y) a = Action.A;
            if (d.x == w.Loc.x + 1 && d.y == w.Loc.y) a = Action.D;
            if (d.y == w.Loc.y - 1 && d.x == w.Loc.x) a = Action.S;
            if (d.y == w.Loc.y + 1 && d.x == w.Loc.x) a = Action.W;
            return a;
        }

        private Action invertAction(Action a) {
            if (a == Action.A) return Action.D;
            if (a == Action.D) return Action.A;
            if (a == Action.W) return Action.S;
            if (a == Action.S) return Action.W;
            if (a == Action.Q) return Action.E;
            if (a == Action.W) return Action.Q;
            throw new Exception("Unexpected Action to invert: " + a);
        }
    }
}
