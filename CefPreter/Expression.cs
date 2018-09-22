using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using CefPreter.Exceptions;
namespace CefPreter
{
    class Expression
    {
        private bool stop = false;
        List<Function.Function> funcs;
        List<Expression> expressions;

        
        public static Expression Parse(List<Token> tokens)
        {
            
            if (!tokens[0].IsReserved() && tokens[0].Type != CefType.Function)
                throw new Exception("Unknown function " + tokens[0].Name);

            Expression expression = tokens.ToExpression();
            return expression;
        }

        public Expression(List<Function.Function> funcs)
        {
            this.funcs = funcs;
        }

        public Expression(List<Expression> exs)
        {
            this.expressions = exs;
        }

        public CefMemory RequiredMemory()
        {
            CefMemory Memory = new CefMemory();
            foreach(var func in funcs.OrEmptyIfNull() )
                Memory.Add(Types.Variable.Create(func.GetType().Name, ""));
            return Memory;
        }

        
        public async Task<ExpressionResult> Execute(Browser.Browser Browser, CefMemory Memory, Interpreter.CallBackEventHandler ceh, Action log = null)
        {
            Types.Variable result = null;
            ExpressionResult res = ExpressionResult.OK;
            if(expressions!=null)
                for(int i = 0; i < expressions.Count; i++)
                {
                    if (res == ExpressionResult.CondFalse)
                        continue;
                    else if (stop) return ExpressionResult.Error;
                    else if (expressions[0].funcs[0].GetType().Name == "While")
                    {
                        if (await expressions[i].Execute(Browser, Memory, ceh, log) == ExpressionResult.WhileCondTrue)
                        {
                            i++;
                            res = await expressions[i].Execute(Browser, Memory, ceh, log);
                            i -= 2;
                        }
                        else
                            i++;

                    }

                    else
                    {
                        res = await expressions[i].Execute(Browser, Memory, ceh, log);
                    }

                }

            foreach(Function.Function func in funcs.OrEmptyIfNull())
            {
                if (stop) return ExpressionResult.Error;
                if (func.GetType().Name == "CLBCK" && ceh != null)
                {
                    ceh(Memory);
                }
                func.Parameters = Memory.UnpackAllVariables(func.Parameters);
                result = await func.Exec(Browser);

                if (func.GetType().Name == "If")
                {
                    if (((Types.Number)result).Value == 0)
                    {
                        res = ExpressionResult.CondFalse;
                    }
                    else
                        res = ExpressionResult.CondTrue;
                }
                else if (func.GetType().Name == "While")
                {
                    if (((Types.Number)result).Value == 1)
                    {
                        res = ExpressionResult.WhileCondTrue;
                    }
                }




                if (result != null)
                    Memory.Set(result);
                if (log != null)
                    log();
            }
            return res;
        }

        public void Stop()
        {
            stop = true;
        }
    }

    public enum ExpressionResult
    {
        OK,
        Error,
        CondTrue,
        CondFalse,
        WhileCondTrue
    }


    static class TokensListExtension
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static Expression ToExpression(this List<Token> tokens)
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
                        throw new WrongParamsCountException();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new WrongParamsCountException();
                    }
                }
                
            }
            return new Expression(funcs);
        }
        


        public static List<Token> Eject(this List<Token> tokens, int i0, int count)
        {

            List<Token> result = new List<Token>();
            count = count == -1 ? tokens.Count - i0 : count;
            for (int i = 0; i < count; i++)
            {
                result.Add(tokens[i0]);
                tokens.RemoveAt(i0);////////////////////////////////removeit
            }
            return result;
        }

        
        

        
    }

    

    
}
