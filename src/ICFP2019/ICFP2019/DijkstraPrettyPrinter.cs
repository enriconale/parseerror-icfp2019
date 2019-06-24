using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace ICFP2019
{
    public class DijkstraPrettyPrinter
    {
        public static void printDijkstraMap(Status status, Wrappy wrappy)
        {
            if (Program.noPrint)
            {
                return;
            }
            Map<Tile> parsedMap = status.map;
            Dictionary<int, Dictionary<int, string>> mappedBoosters = new Dictionary<int, Dictionary<int, string>>();
            List<KeyValuePair<Booster, Point>> boosters = status.boosters;
            foreach (var booster in boosters)
            {
                Point boosterLocation = booster.Value;
                if (!mappedBoosters.ContainsKey(boosterLocation.x))
                {
                    mappedBoosters.Add(boosterLocation.x, new Dictionary<int, string>());
                }

                mappedBoosters[boosterLocation.x].Add(boosterLocation.y, getMappedStringValue(booster.Key));
            }
            System.Console.Out.WriteLine("================ POSIZIONE WRAPPY (" + wrappy.Loc.x + ", " + wrappy.Loc.y + ") ================");
            string usingBooster = "WRAPPY STA USANDO: ";
            if (wrappy.remainingFastWheel > 0)
            {
                usingBooster += "FASTWHEEL, ";
            }

            if (wrappy.remainingDrill > 0)
            {
                usingBooster += "DRILL";
            }
            System.Console.Out.WriteLine(usingBooster);
            if (wrappy.DistMap == null)
            {
                System.Console.Out.WriteLine("================ Could not print dijkstra map ================");
                return;
            }
            for (int i = parsedMap.H - 1; i >= 0; i--)
            {
                System.Console.Write(i % 10 + " ");
                for (int j = 0; j < parsedMap.W; j++)
                {
                    string s = ".";
                    if (wrappy.Loc.x == j && wrappy.Loc.y == i)
                    {
                        s = "?";
                    }
                    else
                    {
                        switch (parsedMap[j, i])
                        {
                            case Tile.Empty:
                                if (mappedBoosters.ContainsKey(j) && mappedBoosters[j].ContainsKey(i))
                                {
                                    s = mappedBoosters[j][i];
                                }
                                else
                                {
                                    int x = wrappy.DistMap[j, i];
                                    if (x < 10) s = x.ToString();
                                    else if (x == int.MaxValue) s = "I";
                                    else s = "+";
                                }
                                break;
                            case Tile.Filled:
                                s = "@";
                                break;
                            case Tile.Obstacle:
                                s = "█";
                                break;
                        }
                    }
                    System.Console.Out.Write(s);
                }
                System.Console.Out.WriteLine("");
            }
            System.Console.Out.Write ("  ");
            for (int i = 0; i < parsedMap.W; ++i)
                System.Console.Out.Write(i % 10);
            System.Console.Out.WriteLine("");
//            System.Console.Out.Write("ActionHistory: ");
//            foreach (var a in wrappy.ActionHistory)
//                System.Console.Out.Write(a);
//            System.Console.Out.WriteLine("");
        }

    public static void printDijkstraMap(Map<int> distMap, Wrappy wrappy)
    {
        if (Program.noPrint)
        {
            return;
        }
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

                System.Console.Out.Write(tileText);
            }
            System.Console.Out.WriteLine("");
        }
        System.Console.Out.WriteLine("");
    }
    
    private static string getMappedStringValue(Booster booster)
    {
        switch (booster)
        {
            case Booster.Drill:
                return "D";
            case Booster.Manipulator:
                return "M";
            case Booster.FastWheels:
                return "F";
            case Booster.Teleport:
                return "T";
            case Booster.Cloning:
                return "C";
            case Booster.CloningPlatform:
                return "X";

            default:
                return "";
        }
    }
}
}