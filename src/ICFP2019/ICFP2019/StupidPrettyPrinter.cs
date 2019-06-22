namespace ICFP2019
{
    public class StupidPrettyPrinter
    {
        public static void printParsedMap(Map<Tile> parsedMap)
        {
            for (int i = parsedMap.H - 1; i >= 0; i--)
            {
                for (int j = 0; j < parsedMap.W; j++)
                {
                    if (parsedMap[j, i] == Tile.Empty)
                    {
                        System.Console.Out.Write("  ");
                    }
                    else
                    {
                        System.Console.Out.Write("██");
                    }
                }
                System.Console.Out.WriteLine("");
            }
        }
    }
}