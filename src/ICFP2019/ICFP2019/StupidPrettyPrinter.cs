using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace ICFP2019
{
    public class StupidPrettyPrinter
    {
        public static void printParsedMap(Map<Tile> parsedMap, List<KeyValuePair<Booster, Point>> boosters, string printerPath)
        {
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(printerPath, false);
            Dictionary<int, Dictionary<int, string>> mappedBoosters = new Dictionary<int, Dictionary<int, string>>();
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
                    else
                    {
                        file.Write("█");
                    }
                }
                file.WriteLine("");
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
                default:
                    return "";
            }
        }
    }
}