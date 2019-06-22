using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICFP2019
{
    class Parser
    {
        private static List<Line> verticalLines = new List<Line>();
        private static List<Line> horizontalLines = new List<Line>();

        public static Status parseProblem(String problem) {
            var le = problem.Split('#');
            List<String> obst = new List<string>();
            for (int i = 2; i < le.Length; i++)
            {
                if (le[i].StartsWith("(")) obst.Add(le[i]);
            }
            var map = Parser.parseMap(le[0], obst);
            var status = new Status(map);

            throw new Exception();
        }

        public static Map<Tile> parseMap(String map, List<String> obstacles) {
            var lines = parseLine(map);
            foreach (var ls in obstacles.Select(x => { return parseLine(x); }))
            {
                lines.AddRange(ls);
            }
            

            throw new Exception();
        }

        public static List<Line> parseLine(String ls) {
            ls = ls.Substring(1, ls.Length - 2);
            var lines = Regex.Split(ls, @"\),\(");
            List<Line> result = new List<Line>();
            var points = lines.ToList().Select<String,Vertex>(x => { return new Vertex(x); }).ToList<Vertex>();
            for (int i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1 ) {
                    result.Add(new Line(points[i], points[0]));
                } else { 
                    result.Add(new Line(points[i], points[i+1]));
                }
            }
            return result;
        }



    }

    class Point
    {
        public int x;
        public int y;

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
    }

    class Vertex : Point
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

        public bool isVertical() {
            return s.x == s.y;
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
