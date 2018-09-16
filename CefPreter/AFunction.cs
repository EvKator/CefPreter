using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefPreter;
using System.Reflection;

namespace CefPreter.Function
{
    abstract class aFunction
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
                    throw new WrongParamsCountException();
                }
                else
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        if (IsFunction(value[i].Type))
                            value[i].Type = CefType.Variable;
                    }
                        
                    this._parameters = value;
                }
            }
        }
        public abstract int ParamsCount { get; protected set; }
        public abstract CefType Type { get; protected set; }

        public aFunction(List<Token> Parameters)
        {
            if (ParamsCount != -1 && ParamsCount != Parameters.Count)
                throw new Exception("Wrong params number");
            this.Parameters = Parameters;
        }

        public aFunction(){}

        public abstract Task<Types.Variable> Exec(Browser Browser);

        public static bool IsFunction(CefType type)
        {
            
            switch (type)
            {
                case CefType.String:
                case CefType.Print:
                case CefType.ToStr:
                case CefType.Navigate:
                case CefType.Click:
                case CefType.Enter:
                case CefType.GoBack:
                case CefType.GoForward:
                case CefType.Reload:
                case CefType.Wait:
                case CefType.InnerHTML:
                case CefType.WaitForElement:
                    return true;
                default:
                    return false;
            }
        }

        public static aFunction Create(Token funcToken)
        {
            aFunction func = null;
            switch (funcToken.Type)
            {
                case CefType.String:
                    func = new Function.String();
                    break;

                case CefType.Number:
                    func = new Function.Number();
                    break;

                case CefType.Print:
                    func = new Function.Print();
                    break;

                case CefType.ToStr:
                    func = new Function.ToStr();
                    break;

                case CefType.Navigate:
                    func = new Function.Navigate();
                    break;

                case CefType.Click:
                    func = new Function.Click();
                    break;

                case CefType.Enter:
                    func = new Function.Enter();
                    break;

                case CefType.GoBack:
                    func = new Function.GoBack();
                    break;

                case CefType.GoForward:
                    func = new Function.GoForward();
                    break;

                case CefType.Reload:
                    func = new Function.Reload();
                    break;

                case CefType.Wait:
                    func = new Function.Wait();
                    break;


                case CefType.InnerHTML:
                    func = new Function.InnerHTML();
                    break;

                case CefType.WaitForElement:
                    func = new Function.WaitForElement();
                    break;

                case CefType.If:
                    func = new Function.WaitForElement();
                    break;

                default:
                    throw new InvalidOperationException("Unknown function " + funcToken.Name);
            }
            return func;
        }
    }

    class Print : aFunction
    {
        public override int ParamsCount { get; protected set; } = -1;
        public override CefType Type { get; protected set; }

        public Print() : base()
        {
            this.Type = CefType.Print;
        }

        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            string message = "";
            foreach (var parameter in Parameters)
            {
                if (parameter.Type == CefType.StringLiteral)
                    message += parameter.Name;
                else
                    throw new Exception("Can't convert " + parameter.Type + " to String");
            }

            return Types.Variable.Create(this.Type.ToString(), message);
        }
    }

    class String : aFunction
    {
        public override int ParamsCount { get; protected set; } = 2;
        public override CefType Type { get; protected set; }

        public String() : base()
        {
            this.Type = CefType.String;
        }

        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            if (!(Parameters[1].Type == CefType.StringLiteral))
                throw new Exception(Parameters[1].Name + " is not a string " + this.ToString());

            return Types.Variable.Create(Parameters[0].Name, Parameters[1].Name);
            
        }
    }

    class Number : aFunction
    {
        public override int ParamsCount { get; protected set; } = 2;
        public override CefType Type { get; protected set; }

        public Number() : base()
        {
            this.Type = CefType.Number;
        }

        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            
            if (!Parameters[1].IsNumberLiteral())
                throw new Exception(Parameters[1].Name + " is not a number " + this.ToString());

            return Types.Variable.Create(Parameters[0].Name, Convert.ToInt32(Parameters[1].Name));
            
        }
    }

    class ToStr : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; }

        public ToStr() : base()
        {
            this.Type = CefType.ToStr;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            Types.Variable result = null;
            if (Parameters[0].Type == CefType.NumberLiteral)
                result = new Types.String(this.Type.ToString(), Parameters[0].ToString());
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");
            
            return result;
        }
    }


    class UserFunction : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; }

        public UserFunction() : base()
        {
            this.Type = CefType.ToStr;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            Types.Variable result = null;
            if (Parameters[0].Type == CefType.StringLiteral)
                result = new Types.String(this.Type.ToString(), Parameters[0].ToString());
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");

            return result;
        }
    }



    class Call : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; }

        public Call() : base()
        {
            this.Type = CefType.ToStr;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            Types.Variable result = null;
            if (Parameters[0].Type == CefType.StringLiteral)
                result = new Types.String(this.Type.ToString(), Parameters[0].ToString());
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");
            
            return result;
        }
    }



    class Navigate : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; }

        public Navigate() : base()
        {
            this.Type = CefType.Navigate;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            string url;
            if (Parameters[0].Type == CefType.StringLiteral)
                url = Parameters[0].Name;
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");
            var result = new Types.String(this.Type.ToString(), url);
            await Browser.Navigate(url);//////////////////////////////////////////////////////////////////////////////
            
            return result;
        }
    }


    class Click : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; }

        public Click() : base()
        {
            this.Type = CefType.Click;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            string url;
            if (Parameters[0].Type == CefType.StringLiteral)
                url = Parameters[0].Name;
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to String");
            var result = new Types.String(this.Type.ToString(), url);
            await Browser.Click(url);
            return result;
        }
    }

    class Enter : aFunction
    {
        public override int ParamsCount { get; protected set; } = 2;
        public override CefType Type { get; protected set; }

        public Enter() : base()
        {
            this.Type = CefType.Enter;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
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


            var result = new Types.String(this.Type.ToString(), xpath);
            await Browser.Enter(xpath, text);
            return result;
        }
    }

    class GoBack : aFunction
    {
        public override int ParamsCount { get; protected set; } = 0;
        public override CefType Type { get; protected set; }
        public GoBack() : base()
        {
            this.Type = CefType.GoBack;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            await Browser.GoBack();
            return null;
        }
    }

    class GoForward : aFunction
    {
        public override int ParamsCount { get; protected set; } = 0;
        public override CefType Type { get; protected set; }
        public GoForward() : base()
        {
           
            this.Type = CefType.GoForward;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            await Browser.GoForward();
            return null;
        }
    }

    class Reload : aFunction
    {
        public override int ParamsCount { get; protected set; } = 0;
        public override CefType Type { get; protected set; }

        public Reload() : base()
        {
            this.Type = CefType.Reload;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            await Browser.Reload();//////////////////////////////////////////////////////////////////////////////
            return null;
        }
    }

    class Wait : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; } = CefType.Wait;

        public Wait() : base()
        {
            this.Type = CefType.Wait;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
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

    class WaitForElement : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; } = CefType.WaitForElement;

        public WaitForElement() : base()
        {
            this.Type = CefType.WaitForElement;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
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

    class If : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;
        public override CefType Type { get; protected set; }

        public If() : base()
        {
            this.Type = CefType.If;
        }

        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            Types.Variable res = null;
            if (Parameters[1].Type == CefType.NumberLiteral)
            {
                if (Convert.ToInt32(Parameters[1]) == 0)
                    res = Types.Variable.Create(this.Type.ToString(), 0);
                else 
                    res = res = Types.Variable.Create(this.Type.ToString(), 1);
            }
            else if (Parameters[1].Type == CefType.String)
            {
                if (System.String.IsNullOrWhiteSpace(Parameters[1].ToString()))
                    res = Types.Variable.Create(this.Type.ToString(), 0);
                else
                    res = res = Types.Variable.Create(this.Type.ToString(), 1);
            }
            else
                throw new Exception(Parameters[1].Name + " is not a number/string " + this.ToString());
            

            return res;

        }
    }

    class InnerHTML : aFunction
    {
        public override int ParamsCount { get; protected set; } = 1;

        public override CefType Type { get; protected set; } = CefType.InnerHTML;

        public InnerHTML() : base()
        {
            this.Type = CefType.InnerHTML;
        }


        public async override Task<Types.Variable> Exec(Browser Browser)
        {
            string xpath = "";

            if (Parameters[0].Type == CefType.StringLiteral)
                xpath = Convert.ToString(Parameters[0].Name);
            else
                throw new Exception("Can't convert " + Parameters[0].Type.ToString() + " to StringLiteral");

            string res = await Browser.GetInnerHTML(xpath);//////////////////////////////////////////////////////////////////////////////


            return Types.Variable.Create(this.Type.ToString(), res);
        }
    }


}
