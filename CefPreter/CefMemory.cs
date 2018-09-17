using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefPreter.Types;
namespace CefPreter
{
    public class CefMemory: ICollection<Types.Variable>, IEnumerable<Types.Variable>///////Cefpreper inner variables scope
    {
        List<Types.Variable> variables;

        public void Update(CefMemory memScope)
        {
            foreach(var variable in memScope)
            {
                Set(variable);
            }
        }

        public void Merge(CefMemory memoryScope)
        {
            foreach (var variable in memoryScope)
            {
                this.Add(variable);
            }
        }

        public int Count
        {
            get
            {
                return ((ICollection<Variable>)variables).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<Variable>)variables).IsReadOnly;
            }
        }

        public CefMemory()
        {
            variables = new List<Types.Variable>();
        }
        
        public override string ToString()
        {
            return string.Join("\r\n", variables.Select(el => el.ToString()));
        }
        
        public void Add(Types.Variable variable)
        {
            variables.Add(variable);
        }

        public bool Set(Types.Variable variable)
        {
            if (!Contains(variable.Name)) Add(variable);
            int i = GetVariableIndex(variable.Name);
            if (variable.Type == CefType.String)
                variables[i] = (Types.String)variable;
            else if(variable.Type == CefType.Number)
                variables[i] = (Types.Number)variable;
            
            if (variables[i].Type == variable.Type)
            {
                
                if(variable.Type == CefType.String)
                    ((Types.String)variables[i]).Value = ((Types.String)variable).Value;
                else if (variable.Type == CefType.Number)
                    ((Types.Number)variables[i]).Value = ((Types.Number)variable).Value;
                else
                    throw new Exception(variable.Type.ToString() + " - wrong type to assign ");

            }
            else throw new Exception(variables[i].Name + " is not a " + variable.Type.ToString());
            return true;
        }
        
        public void Remove(string Name)
        {
            variables.Remove(variables.Find(el => el.Name == Name));
        }
        
        public Types.Variable Get(string Name)
        {
            return variables[GetVariableIndex(Name)];
        }
        

        public void Clear()
        {
            ((ICollection<Variable>)variables).Clear();
        }

        public bool Contains(Variable item)
        {
            return variables.Exists(el => el.Name ==item.Name);
        }

        public bool Contains(string itemName)
        {
            return variables.Exists(el => el.Name == itemName);
        }

        public void CopyTo(Variable[] array, int arrayIndex)
        {
            ((ICollection<Variable>)variables).CopyTo(array, arrayIndex);
        }

        public bool Remove(Variable item)
        {
            return ((ICollection<Variable>)variables).Remove(item);
        }

        public IEnumerator<Variable> GetEnumerator()
        {
            return ((ICollection<Variable>)variables).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Variable>)variables).GetEnumerator();
        }


        public List<Token> UnpackAllVariables(List<Token> startTokens)
        {
            List<Token> tokens = new List<Token>();
            foreach (var token in startTokens)
            {
                if (token.Type == CefType.Variable && (Contains(token.Name)))
                {
                    Token convertedToken = (Token)Get(token.Name);
                    tokens.Add(convertedToken);
                }
                else
                    tokens.Add(token);
            }
            return tokens;
        }

        private int GetVariableIndex(string Name)
        {
            int i = variables.IndexOf(variables.Find(v => v.Name == Name));

            if (i == -1) throw new Exception("Variable " + Name + " not found");

            return i;
        }
    }
}
