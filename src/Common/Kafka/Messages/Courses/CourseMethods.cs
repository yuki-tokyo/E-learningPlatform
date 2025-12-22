using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Kafka.Messages.Courses
{
    public enum CourseMethods
    {
        Add,
        UpdateName,
        UpdateDescription,
        UpdatePrice,
        DeleteCourse
    }
}
