using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    enum CefType
    {
        //TokenTypes
        Undefined,
        Semicolon,
        String,
        Number,
        StringLiteral,
        NumberLiteral,
        Variable,
        Print,
        ToStr,
        Navigate,
        Click,
        Enter,
        GoBack,
        GoForward,
        Reload,
        Wait,
        UFunc,
        UFuncEnd,
        Call,
        EOF,

        ///FunctionsTypes
        
        UFunction,
        InnerHTML,
        WaitForElement,
        If,

        ///Variable types
        Button,
        Link
    }
}
