using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{

    public static class CefExtensions
    {
        public static List<T> Eject<T>(this List<T> tokens, int i0, int count)
        {

            List<T> result = new List<T>();
            count = count == -1 ? tokens.Count - i0 : count;
            for (int i = 0; i < count; i++)
            {
                result.Add(tokens[i0]);
                tokens.RemoveAt(i0);////////////////////////////////removeit
            }
            return result;
        }


        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static Token toToken(this string tok)
        {
            Token t = new Token(tok);
            return t;
        }

        public static Token toToken(this string tok, CefType type)
        {
            Token t = new Token(tok, type);
            return t;
        }


        internal static Expression ToExpression(this List<Token> tokens)
        {
            List<Function.Function> funcs = new List<Function.Function>();
            //List<Token> Params = paramsList(tokens);
            for (int i = tokens.Count - 1; i >= 0; i--)
            {
                if (tokens[i].Type == CefType.Function /*Function.Function.IsFunction(tokens[i].Type)*/)//////////////////////////////////////////
                {
                    Function.Function func = Function.Function.Create(tokens[i]);
                    try
                    {

                        func.Parameters = (tokens.Eject(i + 1, func.ParamsCount));

                        funcs.Add(func);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new CefPreter.Exceptions.CefParamsCountException();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new CefPreter.Exceptions.CefParamsCountException();
                    }
                }

            }
            return new FunctionsExpression(funcs);
        }



        

    }


}
