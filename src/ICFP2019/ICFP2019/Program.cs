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

            if (LaunchMassiveExecution)
            {
                SolveProblemsInFolder();
            }
            else
            {
                SolveSingleProblem(SingleProblemPath);
            }
        }

        private static void SolveSingleProblem(string problemPath)
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
                if (!"".Equals(StupidPrinterOutputPath))
                {
                    StupidPrettyPrinter.printParsedMap(status.map, status.boosters, StupidPrinterOutputPath);
                }
                Solver solver = new Solver(status);
                //solver.solve();
                printSolution(problemPath, problemSolution, solver);
            }
        }

        private static void SolveProblemsInFolder()
        {
            string[] problemFiles = GetFilesInFolder(MassiveProblemsFolderPath);
            int problemsExecuted = 0;

            foreach (string problemFile in problemFiles)
            {
                string filename = problemFile.Substring(System.IO.Path.PathSeparator).Last().ToString();
                filename = filename.Substring(0, filename.Length - 4);
                if (problemsExecuted >= MaxProblemsToExecute)
                {
                    break;
                }

                Task.Run(() =>
                {
                    SolveSingleProblem(MassiveProblemsFolderPath + System.IO.Path.PathSeparator + filename);
                });
                problemsExecuted++;
            }
        }

        private static void printSolution(string problemPath, string problemSolution, Solver solver)
        {
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(problemPath + problemSolution, false);
            List<List<Action>> solution = solver.solution;
            foreach (var sol in solution)
            {
                file.Write("X");
            }
            file.Close();
        }
        
        private static string[] GetFilesInFolder(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);

            return files;
        }

    }
}