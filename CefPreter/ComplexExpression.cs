using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    class ComplexExpression : Expression
    {
        List<Expression> expressions;
        public ComplexExpression(List<Expression> exs) : base()
        {
            this.expressions = exs;
        }

        public async override Task<ExpressionResult> Execute(Browser.Browser Browser, CefMemory Memory, Interpreter.CallBackEventHandler ceh = null, Action log = null)
        {
            ExpressionResult res = ExpressionResult.OK;
            if (expressions != null)
                for (int i = 0; i < expressions.Count; i++)
                {
                    if (stop) return ExpressionResult.Error;
                    res = await expressions[i].Execute(Browser, Memory, ceh, log);

                    switch (res)
                    {
                        case ExpressionResult.CallBack:
                            ceh?.Invoke(Memory);
                            break;
                        case ExpressionResult.CondFalse:
                        case ExpressionResult.WhileCondFalse:
                            res = ExpressionResult.OK;
                            i++;
                            break;
                        case ExpressionResult.WhileCondTrue:
                            res = await expressions[i + 1].Execute(Browser, Memory, ceh, log);
                            i--;
                            break;
                        default:
                            break;
                    }

                }


            return res;
        }
    }
}
