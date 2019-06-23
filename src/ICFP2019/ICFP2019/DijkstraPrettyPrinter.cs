using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace ICFP2019
{
    public class DijkstraPrettyPrinter
    {
        public static void printDijkstraMap(Status status, Wrappy wrappy)
        {
            System.Console.Out.WriteLine("================ Distance map for wrappy at position (" + wrappy.Loc.x + ", " + wrappy.Loc.y + ") ================");
            Map<Tile> parsedMap = status.Map;
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
        }
    }
}