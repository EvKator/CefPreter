using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter.Types
{
    public abstract class Variable
    {
        public virtual string Name { get; }
        public virtual CefType Type { get;  }
        public abstract object Value { get; set; } 
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

        public override object Value { get; set; }
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

        public override object Value { get;set; }
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
}
