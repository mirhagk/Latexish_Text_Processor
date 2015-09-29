using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Latexish_Text_Processor.ParserEngine;
using MacroProviders.Interfaces;

namespace Latexish_Text_Processor.MacroProviders
{
    public class PreprocessProvider:IMacroProvider
    {
        public Parser Parser { set { } }

        public string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            return "[[\\" + CommandName + string.Join("", Parameters.Select(p => "{" + p + "}"))+"]]";
        }
    }
}
