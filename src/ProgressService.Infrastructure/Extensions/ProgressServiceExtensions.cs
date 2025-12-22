using AuthService.Protos;
using Common.Kafka.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProgressService.Domain.Interfaces;
using ProgressService.Infrastructure.gRPC.Clients;
using ProgressService.Infrastructure.Kafka;
using Prometheus.SystemMetrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgressService.Infrastructure.Extensions
{
    public static class ProgressServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            // Prometheus
            services.AddSystemMetrics();

            // gRPC Server
            services.AddGrpc();

            // DI-containers
            // Clients
            services.AddScoped<IAuthClientForProgress, AuthClientForProgress>();

            // Kafka
            services.AddHostedService<KafkaConsumerForTests>();

            services.Configure<KafkaSettings>(
                configuration.GetSection("Kafka"));

            return services;
        }

        public static IServiceCollection AddGrpcClients(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Auth Client
            services.AddGrpcClient<AuthApi.AuthApiClient>(o =>
            {
                o.Address = new Uri("https://localhost:3001");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                // Ignore SSL 
                handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });
            return services;
        }
    }
}
