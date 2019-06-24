using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace ICFP2019
{
    public class StupidPrettyPrinter
    {
        public static void PrintParsedMap(Status currentStatus, string printerPath)
        {
            if (Program.noPrint)
            {
                return;
            }
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(printerPath, false);
            Dictionary<int, Dictionary<int, string>> mappedBoosters = new Dictionary<int, Dictionary<int, string>>();
            Map<Tile> parsedMap = currentStatus.map;
            List<KeyValuePair<Booster, Point>> boosters = currentStatus.boosters;
            foreach (var booster in boosters)
            {
                Point boosterLocation = booster.Value;
                if (!mappedBoosters.ContainsKey(boosterLocation.x))
                {
                    mappedBoosters.Add(boosterLocation.x, new Dictionary<int, string>());
                }

                mappedBoosters[boosterLocation.x].Add(boosterLocation.y, getMappedStringValue(booster.Key));
            }

            for (int i = parsedMap.H - 1; i >= 0; i--)
            {
                for (int j = 0; j < parsedMap.W; j++)
                {
                    if (parsedMap[j, i] == Tile.Empty)
                    {
                        string tileText = " ";
                        if (mappedBoosters.ContainsKey(j) && mappedBoosters[j].ContainsKey(i))
                        {
                            tileText = mappedBoosters[j][i];
                        }
                        file.Write(tileText);
                    }
                    else if (parsedMap[j, i] == Tile.Filled)
                    {
                        file.Write("F");
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
        
        public static void PrintCurrentSemiFilledMap(Status currentStatus)
        {
            Dictionary<int, Dictionary<int, string>> mappedBoosters = new Dictionary<int, Dictionary<int, string>>();
            Map<Tile> parsedMap = currentStatus.map;
            System.Console.Out.WriteLine("====================================================================================================");

            for (int i = parsedMap.H - 1; i >= 0; i--)
            {
                for (int j = 0; j < parsedMap.W; j++)
                {
                    if (currentStatus.wrappies[0].Loc.x == j && currentStatus.wrappies[0].Loc.y == i) {
                        System.Console.Out.Write(currentStatus.wrappies[0].Dir);
                        continue;
                    }
                    if (parsedMap[j, i] == Tile.Empty)
                    {
                        string tileText = " ";
                        System.Console.Out.Write(tileText);
                    }
                    else if (parsedMap[j, i] == Tile.Filled)
                    {
                        System.Console.Out.Write("+");
                    }
                    else
                    {
                        System.Console.Out.Write("█");
                    }
                }
                System.Console.Out.WriteLine("");
            }
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
                    return "W";
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