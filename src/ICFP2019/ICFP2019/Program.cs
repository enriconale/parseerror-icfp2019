using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICFP2019
{
    class Program
    {
        public static void Main(string[] args)
        {
            String problemExtension = ".desc";
            String problemTempSolutions = ".tmp";
            String problemSolution = ".sol";

            if (args.Length == 0)
            {
                System.Console.Out.WriteLine("IL PRIMO ARGOMENTO DEVE ESSERE IL NOME DEL FILE");
                return;
            }

            if (File.Exists(args[0] + problemExtension))
            {
                string problem = File.ReadAllText(args[0] + problemExtension);
                string[] tempSolutions = null;
                if (File.Exists(args[0] + problemTempSolutions))
                {
                    tempSolutions = File.ReadAllLines(args[0] + problemTempSolutions);
                }
                Status status = Parser.parseProblem(problem);
                if (args.Length >= 2 && File.Exists(args[1]))
                {
                    StupidPrettyPrinter.printParsedMap(status.map, status.boosters, args[1]);
                }
                else
                {
                    System.Console.Out.WriteLine("Fornisci (opzionale) un path per il file per il pretty printer");
                }

                StatisticalPrettyPrinter.printStats(status);
            }
        }

    }
}