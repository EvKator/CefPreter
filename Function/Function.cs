using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefPreter;
using CefPreter.Exceptions;
using CefPreter.Browser;
using System.Reflection;

namespace CefPreter.Function
{
    public abstract class Function
    {
        private List<Token> _parameters;
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
                    throw new  WrongParamsCountException();
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
                throw new Exception("Wrong params number");
            this.Parameters = Parameters;
        }

        public Function() { }

        public abstract Task<Types.Variable> Exec(Browser.Browser Browser);

        public static Function Create(Token funcToken)
        {
            Type type = System.Type.GetType("CefPreter.Function." + funcToken.Name);
            object f = Activator.CreateInstance(type);
            return f as Function;
        }
    }

    public class Print : Function
    {
        public override int ParamsCount { get; protected set; } = -1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            string message = "";
            foreach (var parameter in Parameters)
            {
                if (parameter.Type == CefType.StringLiteral)
                    message += parameter.Name;
                else
                    throw new Exception("Can't convert " + parameter.Type + " to String");
            }

            return Types.Variable.Create(this.GetType().Name, message);
        }
    }

    public class AString : Function
    {
        public override int ParamsCount { get; protected set; } = 2;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            if (!(Parameters[1].Type == CefType.StringLiteral))
                throw new Exception(Parameters[1].Name + " is not a string " + this.ToString());

            return Types.Variable.Create(Parameters[0].Name, Parameters[1].Name);

        }
    }

    public class ANumber : Function
    {
        public override int ParamsCount { get; protected set; } = 2;
        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {

            if (!Parameters[1].IsNumberLiteral())
                throw new Exception(Parameters[1].Name + " is not a number " + this.ToString());

            return Types.Variable.Create(Parameters[0].Name, Convert.ToInt32(Parameters[1].Name));

        }
    }

    public class ToStr : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            Types.Variable result = null;
            if (Parameters[0].Type == CefType.NumberLiteral)
                result = new Types.String(this.GetType().Name, Parameters[0].ToString());
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");

            return result;
        }
    }


    public class UserFunction : Function
    {
        public override int ParamsCount { get; protected set; } = 1;
        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            Types.Variable result = null;
            if (Parameters[0].Type == CefType.StringLiteral)
                result = new Types.String(this.GetType().Name, Parameters[0].ToString());
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");

            return result;
        }
    }



    public class Call : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            Types.Variable result = null;
            if (Parameters[0].Type == CefType.StringLiteral)
                result = new Types.String(this.GetType().Name, Parameters[0].ToString());
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");

            return result;
        }
    }



    public class Navigate : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            string url;
            if (Parameters[0].Type == CefType.StringLiteral)
                url = Parameters[0].Name;
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");
            var result = new Types.String(this.GetType().Name, url);
            await Browser.Navigate(url);//////////////////////////////////////////////////////////////////////////////

            return result;
        }
    }


    public class Click : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            string url;
            if (Parameters[0].Type == CefType.StringLiteral)
                url = Parameters[0].Name;
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");
            var result = new Types.String(this.GetType().Name, url);
            await Browser.Click(url);
            return result;
        }
    }

    public class Enter : Function
    {
        public override int ParamsCount { get; protected set; } = 2;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            string xpath = "";
            if (Parameters[0].Type == CefType.StringLiteral)
                xpath = Parameters[0].Name;
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");

            string text = "";
            if (Parameters[1].Type == CefType.StringLiteral)
                text = Parameters[1].Name;
            else
                throw new Exception("Can't convert " + Parameters[1].Type.ToString() + " to String");


            var result = new Types.String(this.GetType().Name, xpath);
            await Browser.Enter(xpath, text);
            return result;
        }
    }

    public class GoBack : Function
    {
        public override int ParamsCount { get; protected set; } = 0;
        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            await Browser.GoBack();
            return null;
        }
    }

    public class GoForward : Function
    {
        public override int ParamsCount { get; protected set; } = 0;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            await Browser.GoForward();
            return null;
        }
    }

    public class Reload : Function
    {
        public override int ParamsCount { get; protected set; } = 0;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            await Browser.Reload();//////////////////////////////////////////////////////////////////////////////
            return null;
        }
    }

    public class Wait : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            int n = 1000;

            if (Parameters[0].Type == CefType.NumberLiteral)
                n = Convert.ToInt32(Parameters[0].Name);
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to Number");

            await Browser.Wait(n);
            return null;
        }
    }

    public class WaitForElement : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            int n = 1000;

            if (Parameters[0].Type == CefType.StringLiteral)
                n = Convert.ToInt32(Parameters[0].Name);
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to Number");

            await Browser.Wait(n);//////////////////////////////////////////////////////////////////////////////


            return null;
        }
    }

    public class If : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            Types.Variable res = null;
            if (Parameters[0].Type == CefType.NumberLiteral)
            {
                int parami = 0;
                try
                {
                    parami = Convert.ToInt32(Parameters[0].Name);
                }
                catch { }
                if (parami == 0)
                    res = Types.Variable.Create(this.GetType().Name, 0);
                else
                    res = res = Types.Variable.Create(this.GetType().Name, 1);
            }
            else if (Parameters[0].Type == CefType.StringLiteral)
            {
                if (System.String.IsNullOrWhiteSpace(Parameters[0].ToString()))
                    res = Types.Variable.Create(this.GetType().Name, 0);
                else
                    res = res = Types.Variable.Create(this.GetType().Name, 1);
            }
            else
                throw new Exception(Parameters[0].Name + " is not a number/string " + this.ToString());

            return res;

        }
    }

    public class While : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            Types.Variable res = null;
            if (Parameters[0].Type == CefType.NumberLiteral)
            {
                int parami = 0;
                try
                {
                    parami = Convert.ToInt32(Parameters[0].Name);
                }
                catch { }
                if (parami == 0)
                    res = Types.Variable.Create(this.GetType().Name, 0);
                else
                    res = res = Types.Variable.Create(this.GetType().Name, 1);
            }
            else if (Parameters[0].Type == CefType.StringLiteral)
            {
                if (System.String.IsNullOrWhiteSpace(Parameters[0].ToString()))
                    res = Types.Variable.Create(this.GetType().Name, 0);
                else
                    res = res = Types.Variable.Create(this.GetType().Name, 1);
            }
            else
                throw new Exception(Parameters[0].Name + " is not a number/string " + this.ToString());

            return res;

        }
    }


    public class InnerHTML : Function
    {
        public override int ParamsCount { get; protected set; } = 1;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            string xpath = "";

            if (Parameters[0].Type == CefType.StringLiteral)
                xpath = Convert.ToString(Parameters[0].Name);
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to StringLiteral");

            string res = await Browser.GetInnerHTML(xpath);

            return Types.Variable.Create(this.GetType().Name, res);
        }
    }

    public class CLBCK : Function
    {
        public override int ParamsCount { get; protected set; } = 0;

        public async override Task<Types.Variable> Exec(Browser.Browser Browser)
        {
            return null;
        }
    }


}
