using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Exceptions
{
    public class CoursePurchaseException : Exception
    {
        public CoursePurchaseException(string message) : base(message) { }
    }
}
