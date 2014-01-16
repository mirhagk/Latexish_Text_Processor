using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    class Program
    {
        static string test = @"
THis is a test

calling a \macro{with a param}{and some \subcommand{with a param}}

\latexish

";
        static string test2 = @"
\newCommand{time}{\now{\timeFormat}}
\newCommand{wrap}{1}{""\arg{1}""}
Today is \now
and the time is \time
\wrap{Hello world}
";
        static void Main(string[] args)
        {
            Console.WriteLine(Parser.Process(test2));
            Console.ReadKey();
        }
    }
}
