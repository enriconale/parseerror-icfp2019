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

        public override string ToString()
        {
            return String.Format("({0}, {1})", x, y);
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


        public static bool FindIntersection(Point pp1, Point pp2, Point qq1, Point qq2, out Point S)
        {
            Tuple<double, double> p1 = new Tuple<double, double>(pp1.x + 0.5, pp1.y + 0.5),
                p2 = new Tuple<double, double>(pp2.x + 0.5, pp2.y + 0.5),
                q1 = new Tuple<double, double>(qq1.x + 0.5, qq1.y + 0.5),
                q2 = new Tuple<double, double>(qq2.x + 0.5, qq2.y + 0.5);

            double a1 = p2.Item2 - p1.Item2,
                b1 = p2.Item1 - p1.Item1,
                c1 = a1 * p1.Item1 + b1 * p1.Item2,
                a2 = q2.Item2 - q1.Item2,
                b2 = q2.Item1 - q1.Item1,
                c2 = a2 * q2.Item2 - b2 * q1.Item2,
                det = a1 * b2 - a2 * b1;

            if (Math.Abs(det) > 1e-8)
            {
                double x = (b2 * c1 - b1 * c2) / det,
                    y = (a1 * c2 - a2 * c1) / det;
                S = new Point((int) x, (int) y);
                return true;
            }

            S = new Point(-1, -1);
            return false;
        }

        public static double FindDistanceToSegment(
                Point ppt, Point pp1, Point pp2)
        {
            Tuple<double, double> pt = new Tuple<double, double>(ppt.x + 0.5, ppt.y + 0.5);
            Tuple<double, double> p1 =  new Tuple<double, double>(pp1.x + 0.5, pp1.y + 0.5);
            Tuple<double, double> p2 = new Tuple<double, double>(pp2.x + 0.5, pp2.y + 0.5);
            Tuple<double, double> closest = new Tuple<double, double>( 0 , 0 );
            double dx = p2.Item1 - p1.Item1;
            double dy = p2.Item2 - p1.Item2;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                dx = pt.Item1 - p1.Item1;
                dy = pt.Item2 - p1.Item2;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            double t = ((pt.Item1 - p1.Item1) * dx + (pt.Item2 - p1.Item2) * dy) /
                (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                dx = pt.Item1 - p1.Item1;
                dy = pt.Item2 - p1.Item2;
            }
            else if (t > 1)
            {
                dx = pt.Item1 - p2.Item1;
                dy = pt.Item2 - p2.Item2;
            }
            else
            {
                closest = new Tuple<double, double>(p1.Item1 + t * dx, p1.Item2 + t * dy);
                dx = pt.Item1 - closest.Item1;
                dy = pt.Item2 - closest.Item2;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

    }
}