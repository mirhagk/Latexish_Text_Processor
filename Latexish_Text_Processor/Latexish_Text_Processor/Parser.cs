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
            public string Text = "";
        }
        public class TextToken : Token { }
        public class CommandToken : Token
        {
            public List<string> Parameters = new List<string>();
        }
        public class IgnoreToken : Token { }
        public class ExcludeToken : Token { }
        public class CommentToken : Token { }
        public Parser()
        {
            MacroProviders = new List<IMacroProvider>();
            MacroProviders.Add(new Command(this));
        }
        private IEnumerable<Token> GetTokens(string input)
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
                    if (input[i] == '\\')
                    {
                        if (i < input.Length + 1 && (input[i+1] == '\n' || input[i+1] == '\r'))
                        {
                            i = GetNextNonWhitespace(input, i).Item2 - 1;
                            continue;
                        }
                        if (textToken.Length != 0)
                            yield return textToken;
                        token = new CommandToken();
                        token.Location = i;
                        token.Line = currentLine;
                        token.Length = 1;
                    }
                    else if (input[i] == '[')
                    {
                        if (textToken.Length != 0)
                            yield return textToken;
                        if (i < input.Length - 1 && input[i + 1] == '[')
                        {
                            if (i < input.Length - 2 && input[i + 2] == '[')
                            {
                                token = new CommentToken();
                                token.Location = i += 2;
                            }
                            else
                            {
                                token = new IgnoreToken();
                                token.Location = ++i;
                            }
                        }
                        else
                        {
                            token = new ExcludeToken();
                            token.Location = i;
                        }
                        token.Line = currentLine;
                        token.Length = 1;
                    }
                    else
                    {
                        textToken.Text += input[i];
                        textToken.Length++;
                    }
                }
                else if (token is CommentToken)
                {
                    if (i < input.Length - 2 && input[i] == ']' && input[i + 1] == ']' && input[i+2] == ']')
                    {
                        token = null;
                        i += 2;
                    }
                }
                else if (token is ExcludeToken || token is IgnoreToken)
                {
                    if (input[i] == ']'&&input[i-1]!='\\')
                    {
                        if (token is ExcludeToken)
                        {
                            yield return token;
                            token = null;
                        }
                        else if (i < input.Length - 1 && input[i + 1] == ']')
                        {
                            yield return token;
                            token = null;
                            i += 1;
                        }
                        else
                            token.Text += input[i];
                    }
                    else
                        token.Text += input[i];
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
                            i--;
                        }
                        else if (commandToken.Length > 1 && input[i] == '{')
                            commandToken.Parameters.Add("");
                        else
                            commandToken.Text += input[i];
                    }
                    else
                    {
                        if (input[i] == '{' && input[i - 1] != '\\')
                            nestedDepth++;
                        if (input[i] == '}' && input[i - 1] != '\\')
                        {
                            if (nestedDepth == 0)
                            {
                                if (GetNextNonWhitespace(input, i).Item1 == '{')
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
        public IEnumerable<Token> Tokenizer(string input)
        {
            return GetTokens(input);
        }
        public string FinalClear(string input)
        {
            return string.Join("\n", input.Split('\n').Where((x) => x.Trim('\r') != ""));
        }
        public List<IMacroProvider> MacroProviders { get; private set; }
        public string ActiveFolder { get; set; }
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
            foreach(var token in GetTokens(input))
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
        private Tuple<char?,int> GetNextNonWhitespace(string input, int position)
        {
            for (int i = position + 1; i < input.Length; i++)
            {
                if (Regex.IsMatch(input[i].ToString(), "\\s"))
                    continue;
                return Tuple.Create<char?,int>(input[i], i);
            }
            return Tuple.Create<char?, int>(null, input.Length);
        }
    }
}
