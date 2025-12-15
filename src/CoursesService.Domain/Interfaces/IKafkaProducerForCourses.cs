using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces
{
    public interface IKafkaProducerForCourses
    {
        Task Produce<T>(T message);
    }
}
