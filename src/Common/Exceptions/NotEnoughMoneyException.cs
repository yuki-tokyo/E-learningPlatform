using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class NotEnoughMoneyException : Exception
    {
        public NotEnoughMoneyException(string message) : base(message) { }
    }
}
