

using System;

namespace ICFP2019
{

    enum Tile
    {
        Empty = 0, Filled = 1, Obstacle = 2
    }

    class Map
    {
        private Tile[,] a;
        private int w, h;

        public Map(int w, int h)
        {
            this.w = w;
            this.h = h;
            a = new Tile[w, h];
            Array.Clear(a, 0, w * h);
        }

    }

    class Status
    {
        private Map map;

        public Status()
        {

        }
}