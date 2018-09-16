using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace CefPreter
{
    class Expression
    {
        List<Function.aFunction> funcs;
        
        public static Expression Parse(List<Token> tokens)
        {
            GetType
            if (!tokens[0].IsReserved())
                throw new Exception("Unknown function " + tokens[0].Name);

            Expression expression = tokens.ToExpression();
            return expression;
        }

        public Expression(List<Function.aFunction> funcs)
        {
            this.funcs = funcs;
        }

        public CefMemory RequiredMemory()
        {
            CefMemory Memory = new CefMemory();
            foreach(var func in funcs)
                Memory.Add(Types.Variable.Create(func.Type.ToString(), ""));
            return Memory;
        }

        
        public async Task<int> Execute(Browser Browser, CefMemory Memory)
        {
            Types.Variable result = null;
            bool hop = false;
            foreach(Function.aFunction func in funcs)
            {
                if (hop)
                    continue;
                if (func.Type == CefType.If)
                {
                    if (((Types.Number)result).Value == 0)
                    {
                        continue;
                    }
                    else
                        hop = true;
                }

                func.Parameters = Memory.UnpackAllVariables(func.Parameters);
                result = await func.Exec(Browser);
                
                if(result != null)
                    Memory.Set(result);
            }
            return 1;
        }
    }


    static class TokensListExtension
    {
        public static Expression ToExpression(this List<Token> tokens)
        {
            List<Function.aFunction> funcs = new List<Function.aFunction>();

            //List<Token> Params = paramsList(tokens);
            for (int i = tokens.Count - 1; i >= 0; i--)
            {
                if (Function.aFunction.IsFunction(tokens[i].Type))
                {
                    Function.aFunction func = Function.aFunction.Create(tokens[i]);
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
