using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor.MacroProviders
{
    public class StandardProvider : CommandProvider
    {
        public StandardProvider()
        {
            AddEscapeMacros();
        }
        private void AddEscapeMacros()
        {
            var replacements = new Dictionary<string, string> { 
            { "n", "\n" }
            };
            var escapeCharacters = "[]{}()";

            macros.AddRange(replacements.Select(x => new Macro()
            {
                Name = x.Key,
                NumParameters = 0,
                LazyParse = false,
                Execute = (_) => x.Value
            }));
            macros.AddRange(escapeCharacters.Select(x => new Macro()
            {
                Name = x.ToString(),
                NumParameters = 0,
                LazyParse = false,
                Execute = (_) => x.ToString()
            }));
        }
        [Macro(false)]
        public string now(string format)
        {
            return DateTime.Now.ToString(format);
        }
        [Macro(false)]
        public string include(string filename)
        {
            filename = Path.HasExtension(filename) ? filename : Path.ChangeExtension(filename, ".texi");

            List<string> locationsToLook = new List<string>();
            locationsToLook.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            locationsToLook.Add(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));
            locationsToLook.Add(Environment.CurrentDirectory);
            locationsToLook.Add(Parser.ActiveFolder);
            locationsToLook = locationsToLook.Where(l => l != null).ToList();

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
        public string newCommand(string name, string numParameters, string lazyParsed, string text)
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
