using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    partial class Command
    {
        public class Macro
        {
            public string Name { get; set; }
            public bool LazyParse { get; set; }
            public int NumParameters { get; set; }
            public Func<string[], string> Execute;
        }
        static Command()
        {
            AddSystemMacros();
            AddEscapeMacros();
        }
        private static Func<string[], string> CreateWrapper(MethodInfo method)
        {
            return (arguments)=>
                {
                    return method.Invoke(null, arguments).ToString();
                };
        }
        private static void AddSystemMacros()
        {
            var methods = typeof(Command).GetMethods().Where((x) => x.CustomAttributes.Any((y) => y.AttributeType == typeof(MacroAttribute)));
            macros.AddRange(methods.Select((x) => new Macro { Name = "system."+x.Name, LazyParse = (x.GetCustomAttribute(typeof(MacroAttribute)) as MacroAttribute).LazyArguments, NumParameters = x.GetParameters().Length, Execute = CreateWrapper(x) }));
        }
        private static void AddEscapeMacros()
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
                    NumParameters=0,
                    LazyParse=false,
                    Execute = (_)=>x.ToString()
                }));
        }
        public static List<Macro> macros = new List<Macro>();
        public static string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            //find macro that matches the number of arguments
            var macro = macros.FirstOrDefault((x) => x.Name == CommandName && x.NumParameters == Parameters.Length);
            if (macro == null)
                throw new InvalidOperationException("Macro " + CommandName + " not found");
            if (!macro.LazyParse)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    Parameters[i] = Parser.Process(Parameters[i]);
                }
            }
            return macro.Execute(Parameters);
        }
    }
}
