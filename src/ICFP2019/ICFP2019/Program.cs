﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ICFP2019
{
    partial class Program
    {
        public const bool noPrint = true;
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
            Console.WriteLine("FINISH");
            Console.ReadKey();
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
                solver.Loop();
                PrintSolution(problemPath, problemSolution, status);
            }
        }

        private static void SolveProblemsInFolder()
        {
            string[] problemFiles = GetFilesInFolder(MassiveProblemsFolderPath);
            int problemsExecuted = 0;

            foreach (string problemFile in problemFiles)
            {
                string filename = problemFile.Substring(Path.PathSeparator).Last().ToString();
                filename = filename.Substring(0, filename.Length - 4);
                if (problemsExecuted >= MaxProblemsToExecute)
                {
                    break;
                }

                Task.Run(() => { SolveSingleProblem(MassiveProblemsFolderPath + Path.PathSeparator + filename); });
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

                        action = tmpSolutionString.Substring(i, j - i);
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

        private static void PrintSolution(string problemPath, string problemSolution, Status status)
        {
            StreamWriter file =
                new StreamWriter(problemPath + problemSolution, false);
            List<Wrappy> wrappies = status.wrappies;
            int wrappiesCount = wrappies.Count();
            for (int i = 0; i < wrappiesCount; i++)
            {
                List<Action> wrappiesActions = wrappies[i].ActionHistory;
                foreach (Action action in wrappiesActions)
                {
                    if (action.IsB || action.IsR)
                    {
                        string strAction = "";
                        if (action.IsB)
                        {
                            Action.B b = (Action.B) action;
                            strAction = "B(" + b.Item1 + "," + b.Item2 + ")";
                        }
                        else
                        {
                            Action.R r = (Action.R) action;
                            strAction = "R(" + r.Item1 + "," + r.Item2 + ")";
                        }
                        file.Write(strAction);
                    }
                    else
                    {
                        file.Write(action.ToString());
                    }
                }

                if (i < wrappiesCount - 1)
                {
                    file.Write("#");
                }
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