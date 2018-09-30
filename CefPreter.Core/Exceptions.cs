using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefPreter.Exceptions
{
    public class CefParamsCountException : Exception
    {
        public CefParamsCountException() : base("Wrong number of parameters")
        {

        }
        public CefParamsCountException(string msg) : base(msg)
        {

        }
    }

    public class CefUnknownFunctionException : Exception
    {
        public CefUnknownFunctionException() : base("Unknown cef function")
        {

        }
        public CefUnknownFunctionException(string msg) : base(msg)
        {

        }
    }

    public class CefTypeException : Exception
    {
        public CefTypeException() : base("Unknown cef type")
        {

        }
        public CefTypeException(string msg) : base(msg)
        {

        }
    }

    public class CefTimeoutExpiredExcepton : Exception
    {
        public CefTimeoutExpiredExcepton() : base("Timeout expired")
        {

        }
        public CefTimeoutExpiredExcepton(string msg) : base(msg)
        {

        }
    }
}
