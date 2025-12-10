using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Exceptions
{
    public class VerificationException : Exception
    {
        public VerificationException(string message) : base(message) { }
    }
}
