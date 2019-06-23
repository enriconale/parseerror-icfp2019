using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace ICFP2019
{
    public class DijkstraPrettyPrinter
    {
        public static void printDijkstraMap(Map<Tile> parsedMap, Wrappy wrappy, string printerPath)
        {
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(printerPath, false);
            file.WriteLine("================ Distance map for wrappy at position (" + wrappy.Loc.x + ", " + wrappy.Loc.y + ") ================");
            file.WriteLine("================ Wrappy is identified by a ● on the map ================");

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
                        
                        file.Write(tileText);
                    }
                    else
                    {
                        file.Write("█");
                    }
                }
                file.WriteLine("");
            }
            file.Close();
        }
    }
}