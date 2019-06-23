
using ICFP2019.Dijkstra;
using System;
using System.Collections.Generic;

namespace ICFP2019
{

    public enum Tile
    {
        Empty = 0, Filled = 1, Obstacle = 2
    }



    public class Map<T> where T : struct
    {
        private T[,] a;
        private int w, h;

        public Map(int w, int h)
        {
            this.W = w;
            this.H = h;
            a = new T[w, h];
            Array.Clear(a, 0, w * h);
        }

        public T this[Point p]
        {
            get
            {
                return this[p.x, p.y];
            }
            set
            {
                this[p.x, p.y] = value;
            }
        }

        public T this[int x, int y]
        {
            get
            {
                return a[x, y];
            }

            set
            {
                a[x, y] = value;
            }
        }

        public int W { get => w; set => w = value; }
        public int H { get => h; set => h = value; }

        public void fillWith(T t)
        {
            for (int i = 0; i < W; ++i)
                for (int j = 0; j < H; ++j)
                    a[i, j] = t;
        }

        public bool validCoordinate(Point p)
        {
            return (p.x >= 0 && p.x < W && p.y >= 0 && p.y < h);
        }
    }

    public enum Dir
    {
        N = 0, E = 1, S = 2, W = 3
    }

    public enum Booster
    {
        Manipulator, FastWheels, Teleport, Drill, Cloning, CloningPlatform
    }

    public partial class Wrappy
    {
        private Point loc;
        private Dir dir;
        private List<Point> manips;
        private List<Action> actionHistory = new List<Action>();
        private Map<int> distMap;
        public List<PriGoal> priGoals;

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

        public void rotateClockwise()
        {
            dir = (Dir)(((int)dir + 1) % 4);
        }

        public void rotateAntiClockwise()
        {
            dir = (Dir)(((int)dir + 3) % 4);
        }

        public Point absolutePosition(Point p)
        {
            int x = Loc.x, y = Loc.y;
            switch (Dir)
            {
                case Dir.E: x += p.x; y += p.y; break;
                case Dir.N: x += -p.y; y += p.x; break;
                case Dir.W: x += -p.x; y += -p.y; break;
                case Dir.S: x += p.y; y += -p.x; break;
            }
            return new Point(x, y);
        }

        private List<Point> ShortestPath(Point dst)
        {
            List<Point> r = new List<Point>();
            if (distMap[dst] == Graph.UNREACHABLE)
                r.Add(dst);
            for (Point p; !dst.Equals(Loc);)
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

        public void updateDistMap(Map<Tile> map, List<Goal> goals)
        {
            Graph graph = new Graph(map);
            Graph.Result res = graph.CalculateMap(this, goals);
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
            foreach (var priGoal in priGoals)
            {
                Goal.GoTo goTo = (Goal.GoTo)priGoal.goal;
                List<Point> path = ShortestPath(new Point(goTo.Item1, goTo.Item2));
                Point p1 = path[0];
                Candidate c = new Candidate { priGoal = priGoal, path = path };
                if (p1.x == Loc.x - 1 && p1.y == Loc.y) W.Add(c);
                if (p1.x == Loc.x + 1 && p1.y == Loc.y) E.Add(c);
                if (p1.y == Loc.y - 1 && p1.x == Loc.x) S.Add(c);
                if (p1.y == Loc.y + 1 && p1.x == Loc.x) N.Add(c);
            }

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

    public struct PriGoal
    {
        public Goal goal;
        public int pri;
    }

    public partial class Status
    {
        public Map<Tile> map;
        public List<Wrappy> wrappies;
        public readonly List<KeyValuePair<Booster, Point>> boosters;
        public List<Booster> collectedBoosters = new List<Booster>();
        public List<Goal> goals;

        public Status(Map<Tile> map, Point wrappyLoc, List<KeyValuePair<Booster, Point>> boosters)
        {
            this.map = map;
            this.wrappies = new List<Wrappy> { new Wrappy(wrappyLoc) };
            this.boosters = boosters;
            // TODO: calculare i corner della mappa qui o ce li facciamo passare in construzione?
        }

        public Map<Tile> Map { get => map; set => map = value; }
    }
}