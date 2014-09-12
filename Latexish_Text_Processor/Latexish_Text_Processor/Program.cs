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
        static string Extension = ".txt.";
        static List<string> FilesToProcess = new List<string>();
        static void Main(string[] args)
        {
            var parser = new Parser();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    switch (args[i].Substring(2).ToLowerInvariant())
                    {
                        case "preprocess":
                            parser.MacroProviders.Add(new PreprocessProvider());
                            break;
                    }
                }
                else if (args[i].StartsWith("-"))
                {
                    if (i > args.Length - 2)
                    {
                        Console.Error.WriteLine("No value specified for command line switch {0}", args[i]);
                        return;
                    }
                    switch (args[i].Substring(1).ToLowerInvariant())
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
                File.WriteAllText(Path.ChangeExtension(file, Extension), parser.Process(File.ReadAllText(file), new string[] { "standardLibrary" }));
            }
            Console.WriteLine("done");
        }
    }
}
