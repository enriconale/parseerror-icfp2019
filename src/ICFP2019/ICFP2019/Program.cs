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

            if (File.Exists(ProblemPath + problemExtension))
            {
                string problem = File.ReadAllText(ProblemPath + problemExtension);
                string[] tempSolutions = null;
                if (File.Exists(ProblemPath + problemTempSolutions))
                {
                    tempSolutions = File.ReadAllLines(ProblemPath + problemTempSolutions);
                }
                Status status = Parser.parseProblem(problem);
                if (!"".Equals(StupidPrinterOutputPath))
                {
                    StupidPrettyPrinter.printParsedMap(status.map, status.boosters, StupidPrinterOutputPath);
                }

                StatisticalPrettyPrinter.printStats(status);
            }
        }
    }
}