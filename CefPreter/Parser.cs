using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    class Parser////////////////Takes string of source code, parses it into the expressions list
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
                /*if (token.Current.Type == CefType.UFunc)
                {
                    Parser parser = new Parser(token, CefType.UFuncEnd);
                    this.Expressions.Add(Expression.Parse(ExTokens));
                    ExTokens.Clear();
                }
                else*/ if (token.Current.Type == CefType.Semicolon)
                {
                    Expressions.Add(Expression.Parse(ExTokens));
                    ExTokens.Clear();
                }
                else
                {
                    ExTokens.Add(token.Current);
                }
            } while (token.Current.Type != stopToken && token.MoveNext());
            return Expressions;
        }


        public void AddExpression()
        {
            
        }
    }
}
