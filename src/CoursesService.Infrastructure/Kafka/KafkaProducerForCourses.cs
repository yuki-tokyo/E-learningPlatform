using Common.Kafka.Settings;
using Confluent.Kafka;
using CoursesService.Domain.Interfaces.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CoursesService.Infrastructure.Kafka
{
    public class KafkaProducerForCourses : IKafkaProducerForCourses, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerForCourses(IOptions<KafkaSettings> kafkaSettings)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = kafkaSettings.Value.Topics.CoursesTopic;
        }

        public async Task Produce<T>(T message)
        {
            try
            {
                var jsonMessage = JsonSerializer.Serialize(message);
                var kafkaMessage = new Message<Null, string>
                {
                    Value = jsonMessage
                };

                var deliveryResult = await _producer.ProduceAsync(_topic, kafkaMessage);
                Console.WriteLine($"Сообщение доставлено в:{deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Ошибка отправки сообщения: {e.Error.Reason}");
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
