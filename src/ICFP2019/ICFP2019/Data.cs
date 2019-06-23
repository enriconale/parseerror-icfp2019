using System;

namespace ICFP2019
{

    public enum Tile
    {
        Empty = 0, Filled = 1, Obstacle = 2
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

    public class Point
    {
        public int x;
        public int y;

        public Point()
        {
            this.x = 0;
            this.y = 0;
        }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(Action a)
        {
            if (a.IsW) { this.x = 0; this.y = 1; };
            if (a.IsA) { this.x = -1; this.y = 0; };
            if (a.IsD) { this.x = 1; this.y = 0; };
            if (a.IsS) { this.x = 0; this.y = -1; };
        }

        public Point(String coords)
        {
            var cs = coords.Split(',');
            if (cs.Length != 2) throw new Exception("Point coords parsing error: " + coords);
            try
            {
                this.x = int.Parse(cs[0]);
                this.y = int.Parse(cs[1]);
                //if float add 0.5
            }
            catch (Exception e)
            {
                //e.Message = "Point coords parsing error: " + coords + " \n" + e.Message;
                throw e;
            }
        }

        public Point(Goal.GoTo GoTo)
        {
            this.x = GoTo.Item1;
            this.y = GoTo.Item2;
        }

        public bool isCollinear(Point other)
        {
            return x == other.x || y == other.y;
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return point != null &&
                   x == point.x &&
                   y == point.y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public int l1norm()
        {
            return Math.Abs(this.x) + Math.Abs(this.y);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }

        private static Point calculateMove(Action a)
        {
            Point m = null;//TODO use switch?
            if (a.IsW) m = new Point(0, 1);
            if (a.IsA) m = new Point(-1, 0);
            if (a.IsD) m = new Point(1, 0);
            if (a.IsS) m = new Point(0, -1);
            return m;
        }

    }
}