using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rookie.Ecom.Contracts.Exceptions
{
    public class ErrorException : Exception
    {
        public ErrorException() : base()
        {
        }

        public ErrorException(string message) : base(message)
        {
        }

        public ErrorException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
