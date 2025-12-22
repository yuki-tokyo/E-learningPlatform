using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Domain.Interfaces.Kafka
{
    public interface IKafkaProducerForTests
    {
        Task Produce<T>(T message);
    }
}
