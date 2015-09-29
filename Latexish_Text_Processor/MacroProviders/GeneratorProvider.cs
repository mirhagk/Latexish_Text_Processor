using Latexish_Text_Processor.MacroProviders;
using Latexish_Text_Processor.ParserEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroProviders
{
    class GeneratorProvider:IMacroProvider
    {
        public Parser Parser { set { } }

        public string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            throw new NotImplementedException();
        }
    }
}
