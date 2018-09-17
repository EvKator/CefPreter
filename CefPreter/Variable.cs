using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter.Types
{
    public abstract class Variable
    {
        public string Name { get; private set; }
        public CefType Type { get; private set; }
        public string Value { get; set; }  = "Variable";
        public Variable(string Name, CefType type)
        {
            this.Name = Name;
            this.Type = type;
        }
        
        public static Variable Create(string Name, int Value)
        {
            return new Number(Name, Value);
        }

        public static Variable Create(string Name, string Value)
        {
            return new String(Name, Value);
        }

        public static explicit operator Token(Variable var)
        {
            Token token;
            if (var.Type == CefType.String)
                token = new Token(((String)var).Value, CefType.StringLiteral);
            else if (var.Type == CefType.Number)
                token = new Token(((Number)var).Value.ToString(), CefType.NumberLiteral);
            else
                throw new NotImplementedException();
            return token;
        }


        public virtual Variable ToStr()
        {
            return new String(this.Name, "Variable");
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.Name, "Variable");
        }

    }



    public class String : Variable
    {

        public static explicit operator Number(String var)
        {
            Number res = null;
            try
            {
                if (var.Type == CefType.String)
                    res = new Number(var.Name, Convert.ToInt32(var.Value));
            }
            catch
            {
                throw new InvalidCastException(var.Value + " is not valid number");
            }
            return res;
        }

        public new string Value { get; set; }
        public String(string Name, string Value = "") : base(Name, CefType.String)
        {
            this.Value = Value;
        }

        public override Variable ToStr()
        {
            return new String(this.Name, this.Value.ToString());
        }
        public override string ToString()
        {
            return string.Format("{0}: {1}", this.Name, this.Value);
        }
    }

    public class Number : Variable
    {
        public static explicit operator String(Number var)
        {
            String res = new String(var.Name, var.Value.ToString());
            return res;
        }

        public new int Value { get;set; }
        public Number(string Name, int Value = 0) : base(Name, CefType.Number)
        {
            this.Value = Value;
        }
        public override Variable ToStr()
        {
            return new String(this.Name, this.Value.ToString());
        }
        public override string ToString()
        {
            return string.Format("{0}: {1}", this.Name, this.Value);
        }
    }

    public class UFunc : Variable
    {
        public new int Value { get; set; }
        public UFunc(string Name, int Value = 0) : base(Name, CefType.Number)
        {
            this.Value = Value;
        }
        public override Variable ToStr()
        {
            return new String(this.Name, this.Value.ToString());
        }
        public override string ToString()
        {
            return string.Format("{0}: {1}", this.Name, this.Value);
        }
    }
}
