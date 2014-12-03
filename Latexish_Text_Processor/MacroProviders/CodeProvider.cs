using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor.MacroProviders
{
    public class CodeProvider : CommandProvider
    {
        public CodeProvider(ParserEngine.Parser parser) : base(parser) { }
        [Macro(false)]
        public string If(string condition, string ifResult, string elseResult)
        {
            if (condition.Trim() == "1")
                return ifResult;
            return elseResult;
        }
        [Macro]
        public string While(string condition, string body)
        {
            string result = "";
            while (parser.Process(condition).Trim() == "1")
            {
                result += body;
            }
            return result;
        }
        private Dictionary<string, string> variables = new Dictionary<string, string>();
        [Macro(false)]
        public string GetVar(string varName)
        {
            if (!variables.ContainsKey(varName))
                return "";
            return variables[varName];
        }
        [Macro(false)]
        public string SetVar(string varName, string value)
        {
            if (!variables.ContainsKey(varName))
                variables.Add(varName, value);
            variables[varName] = value;
            return "";
        }
        [Macro]
        public string SetVarLazy(string varName, string value)
        {
            varName = parser.Process(varName);
            return SetVar(varName, value);
        }
        [Macro(false)]
        public string Equals(string a, string b)
        {
            return (a == b)?"1":"0";
        }
    }
}
