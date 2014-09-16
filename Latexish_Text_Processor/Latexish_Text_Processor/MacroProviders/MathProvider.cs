using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latexish_Text_Processor.MacroProviders
{
    /// <summary>
    /// This provider provides access to mathematical functions, allowing you to do basic math in the document
    /// </summary>
    class MathProvider :IMacroProvider
    {
        public Dictionary<string, Func<double, double, double>> BinaryFunctions = new Dictionary<string, Func<double, double, double>>
        {
            {"Add",(a,b)=>a+b },
            {"Subtract",(a,b)=>a-b },
            {"Multiply",(a,b)=>a*b },
            {"Divide",(a,b)=>a/b },
            {"Percentage",(a,b)=>a/100*b },
        };
        public Dictionary<string, Func<double, double>> UnaryFunctions = new Dictionary<string, Func<double, double>>
        {
        };
        public Dictionary<string, Func<double>> Constants = new Dictionary<string, Func<double>>
        {
            { "Pi", ()=>Math.PI }
        };
        public ParserEngine.Parser Parser { get; set; }
        public string ExecuteCommand(string CommandName, params string[] Parameters)
        {
            if (!CommandName.StartsWith("Math."))
                return null;
            double input1;
            if (Parameters.Length < 1 || !double.TryParse(Parser.Process(Parameters[0]), out input1))
                input1 = double.NaN;
            double input2;
            if (Parameters.Length < 2 || !double.TryParse(Parser.Process(Parameters[1]), out input2))
                input2 = double.NaN;
            CommandName = CommandName.Substring(5);//remove Math.
            if (BinaryFunctions.ContainsKey(CommandName))
                return BinaryFunctions[CommandName](input1,input2).ToString();
            if (UnaryFunctions.ContainsKey(CommandName))
                return UnaryFunctions[CommandName](input1).ToString();
            if (Constants.ContainsKey(CommandName))
                return Constants[CommandName]().ToString();
            return null;
        }
    }
}
