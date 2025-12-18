using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        public CourseNotFoundException(string message) : base(message) { }
    }
}
