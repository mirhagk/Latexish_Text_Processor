using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    class PreprocessProvider:IMacroProvider
    {
        public string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            return "[[\\" + CommandName + string.Join("", Parameters.Select(p => "{" + p + "}"))+" [] ]]";
        }
    }
}
