using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Latexish_Text_Processor
{
    class Parser
    {
        public abstract class Token
        {
            public int Location = -1;
            public int Length = 0;
            public int Line = -1;
        }
        public class TextToken : Token
        {
            public string Text = "";
        }
        public class CommandToken : Token
        {
            public string Command = "";
            public List<string> Parameters = new List<string>();
        }
        private static IEnumerable<Token> GetTokens(string input)
        {
            Token token = null;
            var currentLine =1;
            int nestedDepth = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (i > 0 && input[i - 1] == '\n')
                    currentLine++;
                if (token == null || token is TextToken)
                {
                    var textToken = token as TextToken;
                    if (textToken == null)
                    {
                        token = textToken = new TextToken() { Location = i, Line = currentLine };
                    }
                    if (input[i] != '\\')
                    {
                        textToken.Text += input[i];
                        textToken.Length++;
                    }
                    else
                    {
                        if (textToken.Length != 0)
                            yield return textToken;
                        token = new CommandToken();
                        token.Location = i;
                        token.Line = currentLine;
                        token.Length=1;
                    }
                }
                else if (token is CommandToken)
                {
                    var commandToken = token as CommandToken;
                    if (commandToken.Parameters.Count == 0)
                    {
                        if (commandToken.Length > 1 && Regex.IsMatch(input[i].ToString(), "\\s"))
                        {
                            yield return commandToken;
                            token = null;
                        }
                        else if (commandToken.Length > 1 && input[i] == '{')
                            commandToken.Parameters.Add("");
                        else
                            commandToken.Command += input[i];
                    }
                    else
                    {
                        if (input[i]=='{'&&input[i-1]!='\\')
                            nestedDepth++;
                        if (input[i] == '}' && input[i - 1] != '\\')
                        {
                            if (nestedDepth == 0)
                            {
                                if (GetNextNonWhitespace(input, i) == '{')
                                {
                                    commandToken.Parameters.Add("");
                                    //skip through and consume that { that was found
                                    for (; input[i] != '{'; i++) { }
                                }
                                else
                                {
                                    yield return commandToken;
                                    token = null;
                                }
                                commandToken.Length++;
                                continue;
                            }
                            else
                                nestedDepth--;
                        }
                        commandToken.Parameters[commandToken.Parameters.Count - 1] += input[i];
                    }
                    commandToken.Length++;
                }
                else
                    throw new FormatException("Unrecognized token");
            }
            if (token != null)
                yield return token;
            yield break;
        }
        public static IEnumerable<Token> Tokenizer(string input)
        {
            return GetTokens(input);
        }
        public static string FinalClear(string input)
        {
            return string.Join("\n", input.Split('\n').Where((x) => x.Trim('\r') != ""));
        }
        public static string Process(string input)
        {
            string result="";
            foreach(var token in GetTokens(input))
            {
                if (token is TextToken)
                    result+=(token as TextToken).Text;
                else
                {
                    var command = token as CommandToken;
                    result+=Process(Command.ExecuteCommand(command.Command,command.Parameters.ToArray()));
                }
            }
            return FinalClear(result);
        }
        private static char? GetNextNonWhitespace(string input, int position)
        {
            for (int i = position + 1; i < input.Length; i++)
            {
                if (Regex.IsMatch(input[i].ToString(), "\\s"))
                    continue;
                return input[i];
            }
            return null;
        }
    }
}
