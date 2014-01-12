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

calling a \macro

\latexish

";
        static void Main(string[] args)
        {
            Parser parser= new Parser();
            foreach (var token in parser.Tokenizer(test))
            {
                Console.WriteLine(token.Type.Name);
                Console.WriteLine(token.Text);
            }
            Console.ReadKey();
        }
    }
}
