using Common.Kafka.Settings;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prometheus.SystemMetrics;
using SearchService.Application.AutoMapper;
using SearchService.Application.Services;
using SearchService.Application.Settings;
using SearchService.Domain.Interfaces;
using SearchService.Infrastructure.Kafka;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.Infrastructure.Extensions
{
    public static class SearchServiceExtensions
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
            services.AddScoped<ICoursesSearchService, CoursesSearchService>();

            // Redis 
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = configuration.GetSection("Redis")["ConnectionString"];

                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException(nameof(connectionString), "Ошибка строки подключения к Redis.");

                return ConnectionMultiplexer.Connect(connectionString);
            });

            services.AddScoped(sp => {
                var redis = sp.GetRequiredService<IConnectionMultiplexer>();
                return redis.GetDatabase(); 
            });

            // Kafka
            services.AddHostedService<KafkaConsumerForCourses>();

            services.Configure<KafkaSettings>(
                configuration.GetSection("Kafka"));

            // Mapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = false;
            }, typeof(SearchProfile).Assembly);

            //Elasticsearch 
            services.Configure<ElasticsearchSettings>(configuration.GetSection("Elasticsearch"));

            services.AddSingleton(sp =>
            {
                var uri = sp.GetRequiredService<IOptions<ElasticsearchSettings>>().Value.Uri;
                return new ElasticsearchClient(new Uri(uri));
            });

            return services;
        }

        public static IServiceCollection AddGrpcClients(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }
    }
}
