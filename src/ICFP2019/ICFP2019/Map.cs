using System;

namespace ICFP2019
{
    public class Map<T> where T : struct
    {
        private T[,] a;
        private int w, h;

        public Map(int w, int h, T x)
        {
            this.W = w;
            this.H = h;
            a = new T[w, h];
            this.fillWith(x);
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
            return validCoordinate(p.x, p.y);
        }

        public bool validCoordinate(int x, int y)
        {
            return (x >= 0 && x < W && y >= 0 && y < h);
        }

        public bool isCorner(Point p, T empty) {
            return isCorner(p.x, p.y, empty);
        }

        public bool isCorner(int x, int y, T empty) {
            return
                ((!validCoordinate(x - 1, y) || isNotEmpty(x - 1, y, empty)) ||
                 (!validCoordinate(x + 1, y) || isNotEmpty(x + 1, y, empty))) &&
                ((!validCoordinate(x, y - 1) || isNotEmpty(x, y - 1, empty)) ||
                 (!validCoordinate(x, y + 1) || isNotEmpty(x, y + 1, empty)))
                ;
        }

        public int countAround(int x, int y, T empty)
        { 
            return countNotEmpty(x - 1, y, empty) + countNotEmpty(x + 1, y, empty) + countNotEmpty(x, y-1, empty) + countNotEmpty(x, y+1, empty);
        }

        private int countNotEmpty(int x, int y, T empty)
        {
            return isNotEmpty(x, y, empty) ? 0 : 1;
        }

        public bool isNotEmpty(Point p, T empty) {
            return isNotEmpty(p.x, p.y, empty);
        }

        public bool isNotEmpty(int x, int y, T empty)
        {
            return !this[x, y].Equals(empty);
        }
    }
}