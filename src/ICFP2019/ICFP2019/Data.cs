
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
    }

    public enum Dir
    {
        N = 0, E = 1, S = 2, W = 3
    }

    public class Wrappy
    {
        private Point loc;
        private Dir dir;
        private List<Point> manips;

        public Wrappy(Point loc)
        {
            this.Loc = loc;
            Dir = Dir.E;
            Manips = new List<Point> { new Point(1, 1), new Point(1, 0), new Point(1, -1) };
        }

        public Point Loc { get => loc; set => loc = value; }
        public Dir Dir { get => dir; set => dir = value; }
        public List<Point> Manips { get => manips; set => manips = value; }

        public void rotateClockwise()
        {
            dir = (Dir) (((int) dir + 1) % 4);
        }

        public void rotateAntiClockwise()
        {
            dir = (Dir)(((int) dir + 3) % 4);
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
    }

    public partial class Status
    {
        private Map<Tile> map;
        private Wrappy wrappy;

        public Status(Map<Tile> map, Point wrappyLoc)
        {
            this.map = map;
            this.wrappy = new Wrappy(wrappyLoc);
        }

        public Map<Tile> Map { get => map; set => map = value; }
    }

}