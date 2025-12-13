using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Exceptions
{
    public class CourseChangesException : Exception
    {
        public CourseChangesException(string message) : base(message) { }
    }
}
