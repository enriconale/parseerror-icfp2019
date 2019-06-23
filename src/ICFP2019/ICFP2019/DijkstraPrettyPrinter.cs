using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace ICFP2019
{
    public class DijkstraPrettyPrinter
    {
        public static void printDijkstraMap(Map<Tile> parsedMap, Wrappy wrappy)
        {
            System.Console.Out.WriteLine("================ Distance map for wrappy at position (" + wrappy.Loc.x + ", " + wrappy.Loc.y + ") ================");
            if (wrappy.DistMap == null)
            {
                System.Console.Out.WriteLine("================ Could not print dijkstra map ================");
                return;
            }
            for (int i = parsedMap.H - 1; i >= 0; i--)
            {
                for (int j = 0; j < parsedMap.W; j++)
                {
                    if (parsedMap[j, i] == Tile.Empty)
                    {
                        string tileText = wrappy.DistMap[j, i].ToString();
                        if (wrappy.Loc.x == j && wrappy.Loc.y == i)
                        {
                            tileText = "●";
                        }

                        System.Console.Out.Write(tileText);
                    }
                    else
                    {
                        System.Console.Out.Write("█");
                    }
                }
                System.Console.Out.WriteLine("");
            }
            System.Console.Out.WriteLine("");
        }

        public static void printDijkstraMap(Map<int> distMap, Wrappy wrappy)
        {
            System.Console.Out.WriteLine("================ Distance map for wrappy at position (" + wrappy.Loc.x + ", " + wrappy.Loc.y + ") ================");
            if (distMap == null)
            {
                System.Console.Out.WriteLine("================ Could not print dijkstra map ================");
                return;
            }
            for (int i = distMap.H - 1; i >= 0; i--)
            {
                for (int j = 0; j < distMap.W; j++)
                {
                    string tileText;
                    if (distMap[j, i] == Dijkstra.Graph.UNREACHABLE)
                        tileText = "█";
                    else tileText = distMap[j, i].ToString();
                    if (wrappy.Loc.x == j && wrappy.Loc.y == i)
                    {
                        tileText = "●";
                    }

                    System.Console.Out.Write(tileText);
                }
                System.Console.Out.WriteLine("");
            }
            System.Console.Out.WriteLine("");
        }
    }
}