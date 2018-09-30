using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    class Parser
    {
        CefType stopToken;
        public Parser( CefType stopToken = CefType.EOF)
        {
            this.stopToken = stopToken;
        }

        public List<Expression> ParseExpressions(List<Token> tokens)
        {
            List<Expression> Expressions = new List<Expression>();
            List<Token> ExTokens = new List<Token>();
            var token = tokens.GetEnumerator();
            token.MoveNext();
            do
            {
                if (token.Current.Type == CefType.Begin)
                {
                    int i0 = tokens.IndexOf(token.Current);
                    var nTokens = tokens.GetRange(i0 + 1, tokens.FindLastIndex(t => t.Type == CefType.End) - i0 - 1);
                    var expressions = Parser.ParseExpressions(nTokens, CefType.End);
                    Expressions.Add(new ComplexExpression(expressions));
                    try
                    {

                        tokens = tokens.GetRange(tokens.FindLastIndex(t => t.Type == CefType.End) + 1, tokens.Count - tokens.FindLastIndex(t => t.Type == CefType.End) - 1);
                        if (tokens.Count == 0)
                            break;
                        token = tokens.GetEnumerator();
                    }
                    catch
                    {
                        break;
                    }
                }
                else if (token.Current.Type == CefType.Semicolon)
                {
                    Expressions.Add(Expression.Parse(ExTokens));
                    ExTokens.Clear();
                }
                else
                {
                    ExTokens.Add(token.Current);
                }
            } while (token.MoveNext());
            return Expressions;
        }

        public static List<Expression> ParseExpressions(List<Token> tokens, CefType stopToken)
        {
            Parser parser = new Parser(stopToken);
            return parser.ParseExpressions(tokens);
        }


        public void AddExpression()
        {
            
        }
    }
}
