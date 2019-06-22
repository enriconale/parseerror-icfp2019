using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFP2019
{
    class Parser
    {
        private static List<Line> verticalLines = new List<Line>();
        private static List<Line> horizontalLines = new List<Line>();

        public static Status parseProblem(String problem) {

            throw new Exception();
        }

        public static Map<Tile> parseMap(String map, List<String> obstacles) {



            throw new Exception();
        }


    }

    public class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(String coords) {
            var cs = coords.Split(',');
            if (cs.Length != 2) throw new Exception("Point coords parsing error: " + coords);
            try
            {
                this.x = int.Parse(cs[0]);
                this.y = int.Parse(cs[1]);
                //if float add 0.5
            }
            catch (Exception e) {
                //e.Message = "Point coords parsing error: " + coords + " \n" + e.Message;
                throw e;                
            }
        }

        public bool isCollinear(Point other)
        {
            return x == other.x || y == other.y;
        }

        public static Point operator+(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }

    }

    public class Vertex : Point
    {
        public Vertex(string coords) : base(coords)
        {
        }
    }

    class Line {
        public Vertex s;
        public Vertex e;

        public Line(Vertex s, Vertex e) {
            if (!s.isCollinear(e)) throw new Exception("Invalid Line Endpoints");
            this.s = s;
            this.e = e;
        }

        public bool intersect(Point p)
        {
            return isBetween(s.x, e.x, p.x) || isBetween(s.y, e.y, p.y);
        }

        public bool isBelow(Point p)
        {
            return (s.x == e.x && p.y <= s.y) || (s.y == e.y && p.x <= s.x);
        }

        public bool isBetween(float a, float b, float v) {
            return (a <= v && v < b) || (a > v && v >= b);
        }

        public bool isBetween(int a, int b, int v)
        {
            return (a <= v && v < b) || (a > v && v >= b);
        }

    }

}
