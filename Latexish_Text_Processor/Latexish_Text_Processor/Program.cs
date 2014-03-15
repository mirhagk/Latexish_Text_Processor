using System;
using System.Collections.Generic;
using System.IO;
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
        static string html = @"[
\newCommand{htmlTag}{2}{<\arg{1}>\arg{2}</\arg{1}>}
[[[totally ignored\]\]\]
]\htmlTag{html}{
    \htmlTag{head}{}\
    \htmlTag{body}{
        \htmlTag{p}{This is a paragraph}
    }
}
";
        static string Extension = ".txt.";
        static List<string> FilesToProcess = new List<string>();
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                }
                else if (args[i].StartsWith("-"))
                {
                    if (i > args.Length - 2)
                    {
                        Console.Error.WriteLine("No value specified for command line switch {0}", args[i]);
                        return;
                    }
                    switch (args[i].Substring(1))
                    {
                        case "e":
                            goto case "extension";
                        case "extension":
                            Extension = args[++i];
                            break;
                        default:
                            Console.Error.WriteLine("Could not understand command line switch {0}", args[i]);
                            return;
                    }
                }
                else
                    FilesToProcess.Add(args[i]);
            }
            foreach (var file in FilesToProcess)
            {
                Console.WriteLine("Processing {0}", file);
                File.WriteAllText(Path.ChangeExtension(file, Extension), Parser.Process(File.ReadAllText(file)));
            }
            Console.WriteLine("done");
            //Console.WriteLine(Parser.Process(test2));
            //Console.ReadKey();
            //Console.WriteLine(Parser.Process(html));
            //Console.ReadKey();
        }
    }
}
