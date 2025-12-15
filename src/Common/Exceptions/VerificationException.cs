using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class VerificationException : Exception
    {
        public VerificationException(string message) : base(message) { }
    }
}
