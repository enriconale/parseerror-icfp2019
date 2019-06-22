using System;
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
            var t = Regex.Split("(6,10),(8,10),(8,1),(11,1),(11,10),(12,10),(12,14),(15,14),(15,15),", @"\),\(");
            System.Console.Out.WriteLine(t);
        }
    }
}
