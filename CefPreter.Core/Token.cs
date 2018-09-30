using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    public class Token////represents the class
    {
        public string Name { get; set; }
        public CefType _Type;
        static string[] funcKeyWords;

        public static explicit operator Token(CefPreter.Types.Variable var)
        {
            Token token;
            if (var.Type == CefType.String)
                token = new Token((string)var.Value, CefType.StringLiteral);
            else if (var.Type == CefType.Number)
                token = new Token(((int)var.Value).ToString(), CefType.NumberLiteral);
            else
                throw new NotImplementedException();
            return token;
        }

        public static void AddKeyWords(string[] keyWords)
        {
            funcKeyWords = keyWords;
        }



        public CefType Type
        {
            get {
                if (_Type == CefType.Undefined)
                {

                    if (Name == "EOF".ToString())
                        return CefType.EOF;

                    else if (Name == ";".ToString())
                        return CefType.Semicolon;

                    else if (IsStringLiteral())
                        return CefType.StringLiteral;
                    else if (IsNumberLiteral())
                        return CefType.NumberLiteral;
                    else if (Name == CefType.String.ToString())
                        return CefType.String;
                    else if (Name == CefType.Number.ToString())
                        return CefType.Number;
                    else if (Name == CefType.Begin.ToString())
                        return CefType.Begin;
                    else if (Name == CefType.End.ToString())
                        return CefType.End;
                    else if (IsFunction(Name))
                        return CefType.Function;
                    ////OBLIGATORILY LAST!!!
                    else if (IsVariable())
                        return CefType.Variable;
                    throw new Exception("Unknown token " + Name);
                }
                else
                    return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        private bool IsFunction(string Name)
        {
            if (funcKeyWords.Contains(Name))
                //System.Reflection.Assembly funcs = System.Reflection.Assembly.Load("Function.dll");

                return true;
            else
                return false;
        }

        public Token(string Name)
        {

            this.Name = Name;
            if (IsStringLiteral())
            {
                //this.Name = Name.Replace("_", " ");
                this.Name = Name.Substring(1, Name.Length - 2);
                this.Type = CefType.StringLiteral;
            }
        }

        public Token(string Name, CefType Type)
        {

            this.Name = Name;
            this.Type = Type;
        }



        public override string ToString()
        {
            return this.Name;// String.Format("{0}", this.Value);
        }


        public bool IsStringLiteral()
        {
            return Name.Length > 0 ? Name[0] == '\'' : false;
        }

        public bool IsNumberLiteral()
        {
            int a = 0;
            return int.TryParse(Name, out a);
        }

        private bool IsVariable()
        {
            return Char.IsLetter(Name[0]);
        }

        public bool IsReserved()
        {
            return Type == CefType.Semicolon ||
                Type == CefType.String ||
                Type == CefType.Number ||
                Type == CefType.Function
                ;
        }

    }

   

    


}
