using System;
using System.Collections.Generic;
using System.Linq;
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
            filename = System.IO.Path.HasExtension(filename) ? filename : System.IO.Path.ChangeExtension(filename, ".texi");
            return System.IO.File.ReadAllText(filename);
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
