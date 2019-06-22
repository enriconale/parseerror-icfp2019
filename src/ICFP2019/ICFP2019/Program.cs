using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICFP2019
{
    partial class Program
    {
        public static void Main(string[] args)
        {
            String problemExtension = ".desc";
            String problemTempSolutions = ".tmp";
            String problemSolution = ".sol";

            if (File.Exists(problemPath + problemExtension))
            {
                string problem = File.ReadAllText(problemPath + problemExtension);
                string[] tempSolutions = null;
                if (File.Exists(problemPath + problemTempSolutions))
                {
                    tempSolutions = File.ReadAllLines(problemPath + problemTempSolutions);
                }
                Status status = Parser.parseProblem(problem);
                if (!"".Equals(stupidPrinterOutputPath))
                {
                    StupidPrettyPrinter.printParsedMap(status.map, status.boosters, stupidPrinterOutputPath);
                }

                StatisticalPrettyPrinter.printStats(status);
            }
        }
    }
}