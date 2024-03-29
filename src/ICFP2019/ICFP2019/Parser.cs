﻿using System;
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
        private static int maxX = 0;
        private static int maxY = 0;

        public static Action parseAction(String a)
        {
            //W | S | A | D | Z | E | Q | B of int * int | F | L | R of int * int | C 
            switch (a)
            {
                case "W":
                    return Action.W;
                case "S":
                    return Action.S;
                case "A":
                    return Action.A;
                case "D":
                    return Action.D;
                case "Z":
                    return Action.Z;
                case "E":
                    return Action.E;
                case "Q":
                    return Action.Q;
                case "F":
                    return Action.F;
                case "L":
                    return Action.L;
                case "C":
                    return Action.C;
                default:
                    return parseComplexAction(a);
            }
        }

        private static Action parseComplexAction(string a)
        {
            var p = parsePoint(a);
            if (a.StartsWith("B"))
            {
                return Action.NewB(p.x, p.y);
            }
            else if (a.StartsWith("R"))
            {
                return Action.NewR(p.x, p.y);
            }
            else
                throw new Exception("Parsing unsupported Action");
        }

        public static Status parseProblem(String problem)
        {
            var problemElements = problem.Split('#');
            var map = Parser.parseMap(problemElements[0], problemElements[2].Split(';').ToList());
            var status = new Status(map, new Point(problemElements[1].Substring(1, problemElements[1].Length - 2)), parseBoosters(problemElements[3]));

            return status;
        }

        private static List<KeyValuePair<Booster, Point>> parseBoosters(String boosters)
        {
            var result = new List<KeyValuePair<Booster, Point>>();
            if ("".Equals(boosters))
            {
                return result;
            }

            var bl = boosters.Split(';');
            foreach (var b in bl)
            {
                var type = b.Substring(0, 1);
                var point = parsePoint(b);
                Booster bt;
                switch (type)
                {
                    case "B":
                        bt = Booster.Manipulator;
                        break;
                    case "F":
                        bt = Booster.FastWheels;
                        break;
                    case "L":
                        bt = Booster.Drill;
                        break;
                    case "R":
                        bt = Booster.Teleport;
                        break;
                    case "X":
                        bt = Booster.CloningPlatform;
                        break;
                    case "C":
                        bt = Booster.Cloning;
                        break;
                    default:
                        throw new Exception("Unidetified booster");
                }
                result.Add(new KeyValuePair<Booster, Point>(bt, point));
            }
            return result;
        }

        public static Point parsePoint(String p)
        {
            return new Point(Regex.Match(p, @"(\-?\d+,\-?\d+)").Value);
        }

        public static Map<Tile> parseMap(String map, List<String> obstacles)
        {
            parseLine(map);
            foreach (var ob in obstacles)
            {
                parseLine(ob);
            }
            var result = new Map<Tile>(maxX, maxY, Tile.Empty);
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    var p = new Point(x, y);
                    var horizLinesBelow = horizontalLines.FindAll(l => { return l.intersect(p) && l.isBelow(p); }).Count;
                    var vertLinesbelow = verticalLines.FindAll(l => { return l.intersect(p) && l.isBelow(p); }).Count;
                    if (horizLinesBelow % 2 == 1 && vertLinesbelow % 2 == 1) result[x, y] = Tile.Empty;
                    else result[x, y] = Tile.Obstacle;
                }
            }

            return result;
        }

        public static void parseLine(String ls)
        {
            if (ls.Length == 0) return;
            ls = ls.Substring(1, ls.Length - 2);
            var lines = Regex.Split(ls, @"\),\(");
            var vertexes = lines.ToList().Select<String, Vertex>(x => { return new Vertex(x); }).ToList<Vertex>();
            for (int i = 0; i < vertexes.Count; i++)
            {
                var v = vertexes[i];
                if (maxX < v.x) maxX = v.x;
                if (maxY < v.y) maxY = v.y;
                Line l;
                if (i == vertexes.Count - 1)
                {
                    l = new Line(vertexes[i], vertexes[0]);
                }
                else
                {
                    l = new Line(vertexes[i], vertexes[i + 1]);
                }
                if (l.isVertical()) verticalLines.Add(l);
                else horizontalLines.Add(l);
            }
        }
    }

    public class Vertex : Point
    {
        public Vertex(string coords) : base(coords)
        {
        }
    }

    class Line
    {
        public Vertex s;
        public Vertex e;

        public Line(Vertex s, Vertex e)
        {
            if (!s.isCollinear(e)) throw new Exception("Invalid Line Endpoints");
            this.s = s;
            this.e = e;
        }

        public bool isVertical()
        {
            return s.x == e.x;
        }


        public bool intersect(Point p)
        {
            return isBetween(s.x, e.x, p.x) || isBetween(s.y, e.y, p.y);
        }

        public bool isBelow(Point p)
        {
            return (s.x == e.x && p.x < s.x) || (s.y == e.y && p.y < s.y);
        }

        private bool isBetween(float a, float b, float v)
        {
            return (a <= v && v < b) || (a > v && v >= b);
        }

        private bool isBetween(int a, int b, int v)
        {
            return (a <= v && v < b) || (a > v && v >= b);
        }

    }

}
