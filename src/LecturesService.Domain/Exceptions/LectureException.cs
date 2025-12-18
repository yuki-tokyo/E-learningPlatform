using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Domain.Exceptions
{
    public class LectureException : Exception
    {
        public LectureException(string message) : base(message) { }
    }
}
