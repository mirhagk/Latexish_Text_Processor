using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Latexish_Text_Processor.ParserEngine;

namespace Latexish_Text_Processor.MacroProviders
{
    class MacroAttribute : Attribute
    {
        public bool LazyArguments;
        public MacroAttribute(bool LazyArguments = true)
        {
            this.LazyArguments = LazyArguments;
        }
    }
    abstract class CommandProvider : IMacroProvider
    {
        public class Macro
        {
            public string Name { get; set; }
            public bool LazyParse { get; set; }
            public int NumParameters { get; set; }
            public Func<string[], string> Execute;
        }
        public CommandProvider(Parser parser)
        {
            this.parser = parser;
            AddSystemMacros();
        }
        protected Parser parser;
        private Func<string[], string> CreateWrapper(MethodInfo method)
        {
            return (arguments)=>
                {
                    return method.Invoke(this, arguments).ToString();
                };
        }
        private void AddSystemMacros()
        {
            var methods = this.GetType().GetMethods().Where((x) => x.CustomAttributes.Any((y) => y.AttributeType == typeof(MacroAttribute)));
            macros.AddRange(methods.Select((x) => new Macro { Name = "system."+x.Name, LazyParse = (x.GetCustomAttribute(typeof(MacroAttribute)) as MacroAttribute).LazyArguments, NumParameters = x.GetParameters().Length, Execute = CreateWrapper(x) }));
        }
        public List<Macro> macros = new List<Macro>();
        public string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            //find macro that matches the number of arguments
            var macro = macros.FirstOrDefault((x) => x.Name == CommandName && x.NumParameters == Parameters.Length);
            if (macro == null)
                return null;
            if (!macro.LazyParse)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    Parameters[i] = parser.Process(Parameters[i]);
                }
            }
            return macro.Execute(Parameters);
        }
    }
}
