using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CefPreter
{
    public class Lexer////Translates code string to the list of tokens, represents a methods for manipulation with list of tokens
    {
        public List<Token> tokens { get; private set; }
        public string code { get; private set; }

        public Lexer(string code)
        {
            this.code = NormalizeCode(code);
            List<string> types = new List<string>();

            Assembly assembly = Assembly.Load("Function");
            foreach (var type in assembly.GetTypes())
            {
                types.Add(type.Name);
            }

            Token.AddKeyWords(types.ToArray());
            this.tokens = ParseTokens(this.code);
        }

        private string NormalizeCode(string code)
        {
            code = code.Replace(";", " ; ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");

            MatchCollection matches = Regex.Matches(code, "'.*?'");
            code = Regex.Replace(code, "('(.*?)')", (e) => { string res = e.Groups[2].Value; while (res.Contains(" ")) res = res.Replace(" ", "∏"); return "'" + res + "'"; });
            //List<string>
            
            for(int i = 0; i < matches.Count; i++)
            {

            }

            while (code.Contains("  ")) { code = code.Replace("  ", " "); }
            return code;
        }

        private List<Token> ParseTokens(string code)
        {
            List<string> sTokens = code.Split(' ').ToList();
            List<Token> tokens = new List<Token>();
            //bool EofFound = false;
            foreach (var sToken in sTokens)
            {
                string stok = sToken;
                Token token = null;
                if (!String.IsNullOrWhiteSpace(sToken))
                {

                    while (stok.Contains("∏")) { stok = stok.Replace("∏", " "); }

                    token = new Token(stok);
                }
                if (token != null)
                {
                    tokens.Add(token);
                    //if (token.Type == CefType.EOF)
                    //    EofFound = true;
                }
                    
                
            }
            //if (!EofFound)
            //    tokens.AddRange(new Token[] { new Token("EOF"), new Token(" ; ")});
            return tokens;
        }
        

        public void AddToken(string stoken)
        {
            Token token = new Token(stoken);
            tokens.Add(token);
        }
    }
}
