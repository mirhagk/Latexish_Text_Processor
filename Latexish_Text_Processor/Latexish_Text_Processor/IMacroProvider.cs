using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor.MacroProviders
{
    public interface IMacroProvider
    {
        /// <summary>
        /// Attempt to execute this command and return the result. Returns null if it cannot find this command
        /// </summary>
        string ExecuteCommand(string CommandName, params string[] Parameters);
        ParserEngine.Parser Parser { set; }
    }
}
