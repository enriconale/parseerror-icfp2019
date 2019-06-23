using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace ICFP2019
{
    public class StatisticalPrettyPrinter
    {
        public static void printStats(Status currentStatus)
        {
            int currentEmptyTiles = 0;
            int currentFilledTiles = 0;
            Map<Tile> parsedMap = currentStatus.map;

            for (int i = parsedMap.H - 1; i >= 0; i--)
            {
                for (int j = 0; j < parsedMap.W; j++)
                {
                    if (parsedMap[j, i] == Tile.Empty)
                    {
                        currentEmptyTiles += 1;
                    }
                    else if (parsedMap[j, i] == Tile.Filled)
                    {
                        currentFilledTiles += 1;
                    }
                }
            }
            int totalTiles = currentFilledTiles + currentEmptyTiles;
            double percentFilled = Math.Round((float)currentFilledTiles / totalTiles * 100);
            System.Console.Out.WriteLine("====================================================================================================");
            System.Console.Out.WriteLine("Totale tiles        : " + totalTiles + " di cui " + currentFilledTiles + " riempite (" + percentFilled + "%)");
            if (currentStatus.collectedBoosters != null)
            {
                System.Console.Out.WriteLine("Drill raccolti      : " + currentStatus.collectedBoosters.FindAll(x => { return x == Booster.Drill; }).Count);
                System.Console.Out.WriteLine("Manipulator raccolti: " + currentStatus.collectedBoosters.FindAll(x => { return x == Booster.Manipulator; }).Count);
                System.Console.Out.WriteLine("FastWheels raccolti : " + currentStatus.collectedBoosters.FindAll(x => { return x == Booster.FastWheels; }).Count);
                System.Console.Out.WriteLine("Teleport raccolti   : " + currentStatus.collectedBoosters.FindAll(x => { return x == Booster.Teleport; }).Count);
                System.Console.Out.WriteLine("Cloning raccolti    : " + currentStatus.collectedBoosters.FindAll(x => { return x == Booster.Cloning; }).Count);
            }
            //foreach (var g in currentStatus.goals)
            //   System.Console.Out.WriteLine(g);

        }
    }
}