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
                List<List<Action>> tempSolutions = new List<List<Action>>();
                if (File.Exists(problemPath + problemTempSolutions))
                {
                    tempSolutions = ParseTempSolution(problemPath, problemTempSolutions);
                }
                Status status = Parser.parseProblem(problem);
                if (!"".Equals(StupidPrinterOutputPath))
                {
                    StupidPrettyPrinter.PrintParsedMap(status, StupidPrinterOutputPath);
                }
                Solver solver = new Solver(status, tempSolutions);
                solver.Init();
                //solver.solve();
                //PrintSolution(problemPath, problemSolution, solver);
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

        private static List<List<Action>> ParseTempSolution(string problemPath, string problemTempSolutions)
        {
            string tmpSolutionString = File.ReadAllLines(problemPath + problemTempSolutions).First();
            List<List<Action>> tempSolutions = new List<List<Action>>();
            List<Action> currentActions = new List<Action>();
            for (int i = 0; i < tmpSolutionString.Length;)
            {
                string action = tmpSolutionString[i].ToString();
                if ("#".Equals(action))
                {
                    tempSolutions.Add(currentActions);
                    currentActions = new List<Action>();
                    i++;
                }
                else
                {
                    if (action == "B" || action == "R")
                    {
                        int j = i;
                        while (tmpSolutionString[j] != ')')
                        {
                            j++;
                        }

                        j++;

                        action = tmpSolutionString.Substring(i, j-i);
                        i = j;
                    }
                    else
                    {
                        i++;
                    }
                    currentActions.Add(Parser.parseAction(action));
                }
            }
            tempSolutions.Add(currentActions);

            return tempSolutions;
        }

        private static void PrintSolution(string problemPath, string problemSolution, Solver solver)
        {
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(problemPath + problemSolution, false);
            List<List<Action>> solution = solver.solution;
            foreach (List<Action> sol in solution)
            {
                foreach (Action action in sol)
                {
                    //TODO write correct actions
                    file.Write("$");
                }
                //TODO do not append # after last action
                file.Write("#");
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