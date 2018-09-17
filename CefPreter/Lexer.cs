using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CefPreter
{
    class Lexer////Translates code string to the list of tokens, represents a methods for manipulation with list of tokens
    {
        public List<Token> tokens { get; private set; }
        
        public Lexer(string code)
        {
            string normalizedCode = NormalizeCode(code);
            this.tokens = ParseTokens(normalizedCode);
        }

        public string NormalizeCode(string code)
        {
            code = code.Replace(";", " ; ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");

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

                    while (stok.Contains("_")) { stok = stok.Replace("_", " "); }

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
