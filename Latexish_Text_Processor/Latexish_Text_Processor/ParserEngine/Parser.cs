using System;
using System.Collections.Generic;
using System.Linq;
using Latexish_Text_Processor.MacroProviders;

namespace Latexish_Text_Processor.ParserEngine
{
    class Parser
    {
        public Parser()
        {
            MacroProviders = new List<IMacroProvider>();
            MacroProviders.Add(new StandardProvider(this));
        }
        public string FinalClear(string input)
        {
            return string.Join("\n", input.Split('\n').Where((x) => x.Trim('\r') != ""));
        }
        public List<IMacroProvider> MacroProviders { get; private set; }
        public string ActiveFolder { get; set; }
        private Tokenizer tokenizer = new Tokenizer();
        private string ExecuteCommand(string commandName, params string[] parameters)
        {
            foreach (var provider in MacroProviders)
            {
                var result = provider.ExecuteCommand(commandName, parameters);
                if (result != null)
                    return result;
                if (parameters.Any() && parameters.First() == "")
                {
                    result = provider.ExecuteCommand(commandName, parameters.Skip(1).ToArray());
                    if (result != null)
                        return result;
                }
            }
            throw new InvalidOperationException("Macro " + commandName + " not found");
        }
        public string Process(string input, string[] includedFiles = null)
        {
            string result = "";
            includedFiles = includedFiles??new string[0];
            foreach(var includedFile in includedFiles)
            {
                Process(ExecuteCommand("system.include", includedFile));
            }
            foreach(var token in tokenizer.GetTokens(input))
            {
                if (token is TextToken)
                    result+=(token as TextToken).Text;
                else if (token is ExcludeToken)
                {
                    Process(token.Text);
                }
                else if (token is IgnoreToken)
                {
                    result += token.Text;
                }
                else if (token is CommandToken)
                {
                    var command = token as CommandToken;
                    result += Process(ExecuteCommand(command.Text, command.Parameters.ToArray()));
                }
                else
                    System.Diagnostics.Debug.Fail("Token was an unexpected token type");
            }
            return result;
        }
    }
}
