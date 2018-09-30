using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using CefPreter.Exceptions;
namespace CefPreter
{
    abstract class Expression
    {
        protected bool stop = false;
        List<Function.Function> funcs;

        public static Expression Parse(List<Token> tokens)
        {
            Expression expression = tokens.ToExpression();
            return expression;
        }

        public Expression()
        {

        }

        public CefMemory RequiredMemory()
        {
            CefMemory Memory = new CefMemory();
            foreach (var func in funcs.OrEmptyIfNull())
                Memory.Add(Types.Variable.Create(func.GetType().Name, ""));
            return Memory;
        }

       
        public abstract Task<ExpressionResult> Execute(Browser.Browser Browser, CefMemory Memory, Interpreter.CallBackEventHandler ceh = null, Action log = null);

       

        public void Stop()
        {
            stop = true;
        }
    }

}
