using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefPreter.Exceptions;

namespace CefPreter.Function
{

    public abstract class Function: ICloneable
    {
        private List<Token> _parameters;
        public Token funcToken;
        public List<Token> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                if (value.Count != this.ParamsCount && ParamsCount != -1)
                {
                    throw new CefParamsCountException();
                }
                else
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        if (value[i].Type == CefType.Function)
                            value[i].Type = CefType.Variable;
                    }

                    this._parameters = value;
                }
            }
        }
        public abstract int ParamsCount { get; protected set; }

        public Function(List<Token> Parameters)
        {
            if (ParamsCount != -1 && ParamsCount != Parameters.Count)
                throw new CefParamsCountException("Wrong params count");
            this.Parameters = Parameters;
        }
        

        public Function() { }

        public abstract Task<Types.Variable> Exec(CefPreter.IBrowser Browser);

        public static Function Create(Token funcToken)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.Load("Function");
            Type type = asm.GetType("CefPreter.Function." + funcToken.Name);
            object f = Activator.CreateInstance(type);
            (f as Function).funcToken = funcToken;
            return f as Function;
        }

        public object Clone()
        {
            var f = Function.Create(funcToken);
            f.Parameters = this.Parameters;
            return f;
        }
    }
}
