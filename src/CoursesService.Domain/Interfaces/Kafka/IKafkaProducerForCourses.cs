using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces.Kafka
{
    public interface IKafkaProducerForCourses
    {
        Task Produce<T>(T message);
    }
}
