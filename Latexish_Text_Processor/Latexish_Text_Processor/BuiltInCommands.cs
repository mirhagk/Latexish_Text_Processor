using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    class MacroAttribute:Attribute
    {
        public bool LazyArguments;
        public MacroAttribute(bool LazyArguments=true)
        {
            this.LazyArguments = LazyArguments;
        }
    }
    partial class Command
    {
        [Macro]
        public static string now()
        {
            return DateTime.Now.ToString(ExecuteCommand("formatDateTime"));
        }
        [Macro(false)]
        public static string now(string format)
        {
            return DateTime.Now.ToString(format);
        }
        [Macro]
        public static string formatDateTime()
        {
            return "yyyy-MM-dd hh:mm";
        }
        [Macro(false)]
        public static string include(string filename)
        {
            filename = Path.HasExtension(filename) ? filename : Path.ChangeExtension(filename, ".texi");

            List<string> locationsToLook = new List<string>();
            locationsToLook.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            locationsToLook.Add(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            locationsToLook.Add(Environment.CurrentDirectory);

            locationsToLook.AddRange(locationsToLook.Select(l => Path.Combine(l, "lib")).ToList());
            foreach(var location in locationsToLook)
            {
                if (File.Exists(Path.Combine(location, filename)))
                {
                    return File.ReadAllText(Path.Combine(location, filename));
                }
            }
            throw new IOException(string.Format("Could not find {0}, searched \"{1}\"", filename, string.Join(", ", locationsToLook)));
        }
        [Macro]
        public static string format()
        {
            return "hh:mm";
        }
        [Macro]
        public static string newCommand(string name, string text)
        {
            macros.Add(new Macro()
                {
                    LazyParse = true,
                    Name = name,
                    NumParameters = 0,
                    Execute = (p) => text
                });
            return "";
        }
        [Macro]
        public static string newCommand(string name, string numParameters, string lazyParsed, string text)
        {
            lazyParsed = Parser.Process(lazyParsed).Trim();
            int numParam = int.Parse(numParameters);
            macros.Add(new Macro()
            {
                LazyParse = lazyParsed=="1",
                Name = name,
                NumParameters = numParam,
                Execute = (p) =>
                    {
                        string result = text;
                        for (int i = 0; i < numParam; i++)
                        {
                            result = result.Replace("#" + (i+1), p[i]);
                        }
                        return result;
                    }
            });
            return "";
        }
    }
}
