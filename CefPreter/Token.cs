using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CefPreter
{
    class Token////represents the class
    {
        public string Name { get; set; }
        public CefType _Type;
        public CefType Type
        {
            get {
                if (_Type == CefType.Undefined)
                {
                    Type type = Type.GetType("sdfdsf");// .GetType(Name);

                    if (Name == "EOF".ToString())
                        return CefType.EOF;
                    else if (Name == CefType.Navigate.ToString())
                        return CefType.Navigate;
                    else if (Name == CefType.Click.ToString())
                        return CefType.Click;
                    else if (Name == CefType.Enter.ToString())
                        return CefType.Enter;
                    else if (Name == CefType.GoBack.ToString())
                        return CefType.GoBack;
                    else if (Name == CefType.GoForward.ToString())
                        return CefType.GoForward;
                    else if (Name == CefType.Reload.ToString())
                        return CefType.Reload;
                    else if (Name == CefType.Wait.ToString())
                        return CefType.Wait;
                    else if (Name == ";".ToString())
                        return CefType.Semicolon;
                    else if (Name == CefType.Print.ToString())
                        return CefType.Print;
                    else if (IsStringLiteral())
                        return CefType.StringLiteral;
                    else if (IsNumberLiteral())
                        return CefType.NumberLiteral;
                    else if (Name == CefType.String.ToString())
                        return CefType.String;
                    else if (Name == CefType.Number.ToString())
                        return CefType.Number;
                    else if (Name == CefType.ToStr.ToString())
                        return CefType.ToStr;
                    else if (Name == CefType.InnerHTML.ToString())
                        return CefType.InnerHTML;
                    else if (Name == CefType.WaitForElement.ToString())
                        return CefType.InnerHTML;
                    ////OBLIGATORILY LAST!!!
                    else if (IsVariable())
                        return CefType.Variable;
                    else throw new Exception("Unknown token " + Name);
                }
                else
                    return _Type;
            }
            set
            {
                _Type = value;
            }
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
            return Name.Length > 0 ? Name[0] == '\'': false;
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

        private bool IsANavigate()
        {
            return Name == CefType.Navigate.ToString();
        }

        public bool IsReserved()
        {
            return Type == CefType.Semicolon ||
                Type == CefType.String ||
                Type == CefType.Number ||
                Type == CefType.Print ||
                Type == CefType.ToStr ||
                Type == CefType.Navigate ||
                Type == CefType.Click ||
                Type == CefType.Enter ||
                Type == CefType.GoBack ||
                Type == CefType.GoForward ||
                Type == CefType.Reload ||
                Type == CefType.Wait||
                Type == CefType.InnerHTML||
                Type == CefType.If
                ;
        }
        
    }

    


}
