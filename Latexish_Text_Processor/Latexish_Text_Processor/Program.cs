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
        static void Main(string[] args)
        {
            foreach (var token in Parser.Tokenizer(test))
            {
                Console.WriteLine(token.GetType().Name);
                Console.WriteLine("---");
                if (token is Parser.TextToken)
                    Console.WriteLine((token as Parser.TextToken).Text);
                else if (token is Parser.CommandToken)
                {
                    Console.WriteLine((token as Parser.CommandToken).Command);
                    foreach (var param in (token as Parser.CommandToken).Parameters)
                    {
                        Console.WriteLine("Param: {0}", param);
                    }
                }
                Console.WriteLine();
                
            }
            Console.ReadKey();
        }
    }
}
