using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    class FunctionsExpression : Expression
    {
        List<Function.Function> functions;



        public FunctionsExpression(List<Function.Function> funcs) : base()
        {
            this.functions = funcs;
        }

        public async override Task<ExpressionResult> Execute(Browser.Browser browser, CefMemory memory, Interpreter.CallBackEventHandler ceh = null, Action log = null)
        {
            ExpressionResult res = ExpressionResult.OK;
            Types.Variable result = null;

            if (functions.Count > 0)
            {
                if (functions[0].GetType().Name == "CLBCK")
                {
                    res = ExpressionResult.CallBack;
                }

                foreach (Function.Function f in functions.OrEmptyIfNull())
                {

                    if (stop) return ExpressionResult.Error;
                    var func = f.Clone() as Function.Function;

                    
                    if (func.GetType().Name == "AString" || func.GetType().Name == "ANumber")
                    {
                        memory.Remove(func.Parameters[0].Name, true);
                    }
                    func.Parameters = memory.UnpackAllVariables(func.Parameters);


                    result = await func.Exec(browser);

                    if (func.GetType().Name == "If")
                    {
                        if ((int)((Types.Number)result).Value == 0)
                        {
                            res = ExpressionResult.CondFalse;
                        }
                        else
                            res = ExpressionResult.CondTrue;
                    }
                    else if (func.GetType().Name == "While")
                    {
                        if ((int)((Types.Number)result).Value == 1)
                        {
                            res = ExpressionResult.WhileCondTrue;
                        }
                        else
                            res = ExpressionResult.WhileCondFalse;
                    }




                    if (result != null)
                        memory.Set(result);
                    log?.Invoke();
                }
            }
            return res;
        }

    }
}
