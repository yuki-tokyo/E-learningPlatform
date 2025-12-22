using Common.Kafka.Messages.Tests;
using Common.Kafka.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProgressService.Domain.Interfaces;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;

namespace ProgressService.Infrastructure.Kafka
{
    public class KafkaConsumerForTests : BackgroundService
    {
        private readonly IConsumer<Ignore, string> consumer;
        private readonly string topic;
        private readonly IServiceScopeFactory scopeFactory;


        private static readonly Counter messagesTotal = Metrics
            .CreateCounter("kafka_courses_messages_total",
                "Всего сообщений", ["method", "status"]);

        private static readonly Histogram processTime = Metrics
            .CreateHistogram("kafka_courses_process_seconds",
                "Время обработки");

        private static readonly Counter errorsTotal = Metrics
            .CreateCounter("kafka_courses_errors_total",
                "Ошибки обработки");

        public KafkaConsumerForTests(
            IOptions<KafkaSettings> kafkaSettings,
            IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;

            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = kafkaSettings.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            topic = kafkaSettings.Value.Topics.CoursesTopic;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    if (consumeResult?.Message?.Value != null)
                    {
                        using var timer = processTime.NewTimer();
                        var method = "unknown";
                        var status = "success";

                        try
                        {
                            using var scope = scopeFactory.CreateScope();

                            var client = scope.ServiceProvider
                                .GetRequiredService<IAuthClientForProgress>();

                            var msg = JsonSerializer.Deserialize<TestMessage>(
                                consumeResult.Message.Value);

                            await client.UpdateUserLevel(msg.UserId, msg.Points);

                            consumer.Commit(consumeResult);
                        }
                        catch (Exception)
                        {
                            status = "error";
                            errorsTotal.Inc();
                            throw;
                        }
                        finally
                        {
                            messagesTotal.WithLabels(method, status).Inc();
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    errorsTotal.Inc();
                    Console.WriteLine($"Ошибка: {e.Error.Reason}");
                }
            }

            consumer.Close();
        }
    }
}
