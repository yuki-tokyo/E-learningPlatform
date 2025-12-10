using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Exceptions
{
    public class VerificationException : Exception
    {
        public VerificationException(string message) : base(message) { }
    }
}
