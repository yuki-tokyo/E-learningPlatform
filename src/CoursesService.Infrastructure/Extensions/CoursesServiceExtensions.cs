using Common.Kafka.Settings;
using CoursesService.Application.AutoMapper;
using CoursesService.Domain.Interfaces.Clients.Lectures;
using CoursesService.Domain.Interfaces.Clients.Payment;
using CoursesService.Domain.Interfaces.Clients.Search;
using CoursesService.Domain.Interfaces.Clients.Tests;
using CoursesService.Domain.Interfaces.Courses;
using CoursesService.Domain.Interfaces.Kafka;
using CoursesService.Infrastructure.Data;
using CoursesService.Infrastructure.gRPC.Clients;
using CoursesService.Infrastructure.Kafka;
using CoursesService.Infrastructure.Repos;
using LecturesService.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentService.Protos;
using Prometheus.SystemMetrics;
using SearchService.Protos;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Protos;
using AppCoursesService = CoursesService.Application.Services.CoursesService;

namespace CoursesService.Infrastructure.Extensions
{
    public static class CoursesServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<CoursesDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(CoursesDb).Assembly.FullName)));

            // DbContextFactory
            services.AddDbContextFactory<CoursesDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(CoursesDb).Assembly.FullName)),
                    lifetime: ServiceLifetime.Scoped);

            // HttpContext
            services.AddHttpContextAccessor();

            // gRPC Server
            services.AddGrpc();

            // Prometheus
            services.AddSystemMetrics();

            // DI-containers
            // Repositories
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            // Services
            services.AddScoped<ICoursesService, AppCoursesService>();
            // Clients
            services.AddScoped<IPaymentClientForCourses, PaymentClientForCourses>();
            services.AddScoped<ISearchClientForCourses, SearchClientForCourses>();
            services.AddScoped<ILecturesClientForCourses, LecturesClientForCourses>();
            services.AddScoped<ITestsClientForCourses, TestsClientForCourses>();
            // Kafka
            services.AddSingleton<IKafkaProducerForCourses, KafkaProducerForCourses>();

            services.Configure<KafkaSettings>(
                configuration.GetSection("Kafka"));

            // Mapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = false;
            }, typeof(CoursesProfile).Assembly);

            // Jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                        )
                    };
                });

            return services;
        }

        public static IServiceCollection AddGrpcClients(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Payment Client
            services.AddGrpcClient<PaymentApi.PaymentApiClient>(o =>
            {
                o.Address = new Uri("https://localhost:3005");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                // Ignore SSL 
                handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });

            // Search Client
            services.AddGrpcClient<SearchApi.SearchApiClient>(o =>
            {
                o.Address = new Uri("https://localhost:3006");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                // Ignore SSL 
                handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });

            // Lectures Client
            services.AddGrpcClient<LecturesApi.LecturesApiClient>(o =>
            {
                o.Address = new Uri("https://localhost:3007");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                // Ignore SSL 
                handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                return handler;
            });

            // Tests Client
            services.AddGrpcClient<TestsApi.TestsApiClient>(o =>
            {
                o.Address = new Uri("https://localhost:3008");
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
