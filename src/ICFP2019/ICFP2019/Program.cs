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
            if (args.Length == 0)
            {
                System.Console.Out.WriteLine("IL PRIMO ARGOMENTO DEVE ESSERE IL NOME DEL FILE");
                return;
            }

            if (File.Exists(args[0]))
            {
                string problem = File.ReadAllText(args[0]);
                Status status = Parser.parseProblem(problem);
                StupidPrettyPrinter.printParsedMap(status.map);
            }
        }
    }
}