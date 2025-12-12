using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Exceptions.Email
{
    public class VerificationException : Exception
    {
        public VerificationException(string message) : base(message) { }
    }
}
