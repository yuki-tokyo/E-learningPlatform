using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class QuestionException : Exception
    {
        public QuestionException(string message) : base(message) { }
    }
}
