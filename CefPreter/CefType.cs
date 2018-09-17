using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    public enum CefType
    {
        //TokenTypes
        Undefined,
        Semicolon,
        String,
        Number,
        StringLiteral,
        NumberLiteral,
        Variable,
        
        Begin,
        End,
        Call,
        EOF,

        ///FunctionsTypes
        Function,
        If,

        ///Variable types
        Button,
        Link
    }
}
