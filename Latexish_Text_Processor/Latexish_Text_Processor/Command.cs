using System;
using System.Collections.Generic;
using System.Linq;
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
            public Func<string[], string> Execute;
        }
        public static List<Macro> macros = new List<Macro>();
        public static string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            var macro = macros.FirstOrDefault((x) => x.Name == CommandName);
            if (macro == null)//do some stuff to find it among included command engines
                throw new InvalidOperationException("Macro " + CommandName + " not found");
            if (!macro.LazyParse)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    Parameters[i] = Parser.Process(Parameters[i]);
                }
            }
            return macro.Execute(Parameters); ;
            //Parser.Tokenizer()
        }
    }
}
