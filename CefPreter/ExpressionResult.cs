using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter
{
    internal enum ExpressionResult
    {
        OK,
        Error,
        CondTrue,
        CondFalse,
        WhileCondTrue,
        WhileCondFalse,
        CallBack
    }
}
