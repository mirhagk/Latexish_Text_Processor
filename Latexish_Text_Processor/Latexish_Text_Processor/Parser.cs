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
        class Token
        {
            public int Location { get; set; }
            /// <summary>
            /// The length of the token as it appears in the source text.
            /// </summary>
            /// <remarks>
            /// This is not neccessarily the same as Text.Length, since Text isn't what appeared in the source, 
            /// but rather the Text that represents the token.
            /// For instance for whitespace it's a single whitespace token
            /// </remarks>
            public int Length { get; set; }
            public int Line { get; set; }
            public string Text { get; set; }
            public TokenType Type { get; set; }
        }
        class TokenType
        {
            public Regex Matching { get; set; }
            public string Name { get; set; }
            private TokenType(string regex)
            {
                Matching = new Regex("^" + regex, RegexOptions.Compiled | RegexOptions.Singleline);
            }
            public static readonly TokenType Text = new TokenType(@"([a-zA-Z0-9]+)");
            public static readonly TokenType Whitespace = new TokenType(@"(\s)\s*");
            public static readonly TokenType Command = new TokenType(@"\\([^ \s{]+)");
            public static readonly TokenType ParamStart = new TokenType(@"{");
            public static readonly TokenType ParamEnd = new TokenType(@"}");
            public static readonly TokenType EOF = new TokenType("$");
            public static List<TokenType> TokenTypes;
            static TokenType()
            {
                TokenTypes = typeof(TokenType).GetFields(BindingFlags.Static | BindingFlags.Public).Select((x) => (TokenType)x.GetValue(null)).ToList();
            }
        }
        private IEnumerable<Token> GetTokens(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                foreach (var tokenType in TokenType.TokenTypes)
                {
                    var match = tokenType.Matching.Match(input, i);
                    if (match.Success)
                    {
                        var token = new Token();
                        token.Text = match.Groups[1].Value;
                        token.Location = i;
                        token.Length = match.Length;
                        //TODO: count the line numbers somehow
                        yield return token;
                        i += match.Length;
                        break;
                    }
                }
            }
            yield break;
        }
        private bool Filter(Token token)
        {
            return !(token is Text && String.IsNullOrEmpty(token.Text));
        }
        public IEnumerable<Token> Tokenizer(string input)
        {
            return GetTokens(input).Where((x) => Filter(x));
        }
    }
}
