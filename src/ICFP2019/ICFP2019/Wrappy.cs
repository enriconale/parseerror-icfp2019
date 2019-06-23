using ICFP2019.Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFP2019
{

    public class Wrappy
    {
        private Point loc;
        private Dir dir;
        private List<Point> manips;
        private List<Action> actionHistory = new List<Action>();
        private Map<int> distMap;
        public List<PriGoal> priGoals;
        public int remainingFastWheel = 0;
        public int remainingDrill = 0;

        public Wrappy(Point loc)
        {
            this.Loc = loc;
            Dir = Dir.E;
            Manips = new List<Point> { new Point(1, 1), new Point(1, 0), new Point(1, -1) };
        }

        public Point Loc { get => loc; set => loc = value; }
        public Dir Dir { get => dir; set => dir = value; }
        public List<Point> Manips { get => manips; set => manips = value; }
        public List<Action> ActionHistory { get => actionHistory; set => actionHistory = value; }

        public Action LastAction
        {
            get
            {
                return ActionHistory.Count == 0 ? null : ActionHistory.Last();
            }
        }

        public Map<int> DistMap => distMap;

        public void rotateClockwise()
        {
            dir = (Dir)(((int)dir + 1) % 4);
            var nm = new List<Point>();
            foreach (var m in this.manips)
            {
                nm.Add(new Point(m.y, -m.x));
            }
            this.manips = nm;
        }

        public void rotateAntiClockwise()
        {
            dir = (Dir)(((int)dir + 3) % 4);
            var nm = new List<Point>();
            foreach (var m in this.manips)
            {
                nm.Add(new Point(-m.y, m.x));
            }
            this.manips = nm;
        }

        public Point absolutePosition(Point p)
        {
            //int x = Loc.x, y = Loc.y;
            //switch (Dir)
            //{
            //    case Dir.E: x += p.x; y += p.y; break;
            //    case Dir.N: x += -p.y; y += p.x; break;
            //    case Dir.W: x += -p.x; y += -p.y; break;
            //    case Dir.S: x += p.y; y += -p.x; break;
            //}
            //return new Point(x, y);
            return new Point(Loc.x + p.x, Loc.y + p.y);
        }

        public Point _relativeRotPosition(Point p)
        {
            int x = 0; 
            int y = 0;
            switch (Dir)
            {
                case Dir.E: x = -p.x; y = -p.y; break;
                case Dir.N: x = p.y; y = p.x; break;
                case Dir.W: x = p.x; y = p.y; break;
                case Dir.S: x = -p.y; y = p.x; break;
            }
            return new Point(x, y);
        }

        private List<Point> ShortestPath(Point dst)
        {
            //Console.WriteLine("dst = " + dst);
            List<Point> r = new List<Point>();
            r.Add(dst);
            Point p;
            for (int i = distMap[dst]; i > 1; --i)
            {
                int d = distMap[dst];
                if (dst.y + 1 <= distMap.H - 1 && distMap[(p = new Point(dst.x, dst.y + 1))] == d - 1
                   || dst.y > 0 && distMap[(p = new Point(dst.x, dst.y - 1))] == d - 1
                   || dst.x > 0 && distMap[(p = new Point(dst.x - 1, dst.y))] == d - 1
                   || dst.x + 1 <= distMap.W - 1 && distMap[(p = new Point(dst.x + 1, dst.y))] == d - 1)
                {
                    r.Insert(0, p);
                    dst = p;
                }
            }
            return r;
        }

        public void updateDistMap(Status status)
        {
            Map<Tile> map = status.map;
            Graph graph = new Graph(map);
            Graph.Result res = graph.CalculateMap(this, status);
            distMap = res.distMap;
            priGoals = res.priGoals;
        }

        public struct Candidate
        {
            public PriGoal priGoal;
            public List<Point> path;
        }

        private double priOfCandidates(List<Candidate> cands)
        {
            double r = 0.0;
            foreach (var cand in cands)
            {
                r += 1.00 / Math.Pow(cand.priGoal.pri, 2.0);
            }
            return r;
        }

        public struct PriPath
        {
            public double pri;
            public List<Point> path;
        }

        private List<Point> findMin(List<Candidate> cands)
        {
            cands.Sort((c1, c2) => c1.priGoal.pri - c2.priGoal.pri);
            return cands[0].path;
        }

        public PriPath BestShortestPath()
        {
            List<Candidate> N = new List<Candidate>();
            List<Candidate> S = new List<Candidate>();
            List<Candidate> E = new List<Candidate>();
            List<Candidate> W = new List<Candidate>();
            Console.Write("wrappy " + Loc + " goals: " );
            foreach (var priGoal in priGoals)
            {
                Goal.GoTo goTo = (Goal.GoTo)priGoal.goal;
                Console.Write(new Point(goTo.Item1, goTo.Item2) + " ");
                List<Point> path = ShortestPath(new Point(goTo.Item1, goTo.Item2));
                Point p1 = path[0];
                Candidate c = new Candidate { priGoal = priGoal, path = path };
                if (p1.x == Loc.x - 1 && p1.y == Loc.y) W.Add(c);
                if (p1.x == Loc.x + 1 && p1.y == Loc.y) E.Add(c);
                if (p1.y == Loc.y - 1 && p1.x == Loc.x) S.Add(c);
                if (p1.y == Loc.y + 1 && p1.x == Loc.x) N.Add(c);
            }
            Console.WriteLine("");
            double pN = priOfCandidates(N);
            double pS = priOfCandidates(S);
            double pE = priOfCandidates(E);
            double pW = priOfCandidates(W);

            if (pN > pS && pN > pW && pN > pE)
                return new PriPath { pri = pN, path = findMin(N) };
            if (pS > pN && pS > pW && pS > pE)
                return new PriPath { pri = pS, path = findMin(S) };
            if (pW > pN && pW > pS && pW > pE)
                return new PriPath { pri = pW, path = findMin(W) };
            else return new PriPath { pri = pE, path = findMin(E) };
        }

    }

}

