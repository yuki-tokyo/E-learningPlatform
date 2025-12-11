using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
    }
}
