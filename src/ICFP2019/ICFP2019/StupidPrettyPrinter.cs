namespace ICFP2019
{
    public class StupidPrettyPrinter
    {
        public static void printParsedMap(Map<Tile> parsedMap, string printerPath)
        {
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(printerPath, false);
            for (int i = parsedMap.H - 1; i >= 0; i--)
            {
                for (int j = 0; j < parsedMap.W; j++)
                {
                    if (parsedMap[j, i] == Tile.Empty)
                    {
                        file.Write("  ");
                    }
                    else
                    {
                        file.Write("██");
                    }
                }
                file.WriteLine("");
            }
        }
    }
}