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

        public bool isCorner(Point p, T empty) {
            return isCorner(p.x, p.y, empty);
        }

        public bool isCorner(int x, int y, T empty) {
            return
                isNotEmpty(x - 1, y, empty) && isNotEmpty(x, y + 1, empty) ||
                isNotEmpty(x + 1, y, empty) && isNotEmpty(x, y + 1, empty) ||
                isNotEmpty(x, y - 1, empty) && isNotEmpty(x + 1, y, empty) ||
                isNotEmpty(x, y - 1, empty) && isNotEmpty(x - 1, y, empty)
                ;
        }

        public int countAround(int x, int y, T empty)
        { 
            return countNotEmpty(x - 1, y, empty) + countNotEmpty(x + 1, y, empty) + countNotEmpty(x, y-1, empty) + countNotEmpty(x, y+1, empty);
        }

        private int countNotEmpty(int x, int y, T empty)
        {
            return (this[x - 1, y].Equals(empty)) ? 0 : 1;
        }

        public bool isNotEmpty(Point p, T empty) {
            return isNotEmpty(p.x, p.y, empty);
        }

        private bool isNotEmpty(int x, int y, T empty)
        {
            return this[x - 1, y].Equals(empty);
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