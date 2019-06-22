﻿
using System;


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

    public class Robot
    {
    }

    public partial class Status
    {
        private Map<Tile> map;
        private Robot robot;

        public Status(Map<Tile> map)
        {
            this.map = map;
        }

        public Map<Tile> Map { get => map; set => map = value; }
    }

}