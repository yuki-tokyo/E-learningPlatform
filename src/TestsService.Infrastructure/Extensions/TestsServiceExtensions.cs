using CoursesService.Protos;
using LecturesService.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prometheus.SystemMetrics;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Application.Services;
using TestsService.Domain.Interfaces.Courses;
using TestsService.Domain.Interfaces.Lectures;
using TestsService.Domain.Interfaces.Questions;
using TestsService.Domain.Interfaces.Tests;
using TestsService.Infrastructure.Data;
using TestsService.Infrastructure.gRPC.Clients;
using TestsService.Infrastructure.Repos;
using AppTestsService = TestsService.Application.Services.TestsService;

namespace TestsService.Infrastructure.Extensions
{
    public static class TestsServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<TestsDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(TestsDb).Assembly.FullName)));

            // DbContextFactory
            services.AddDbContextFactory<TestsDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(TestsDb).Assembly.FullName)),
                    lifetime: ServiceLifetime.Scoped);

            // HttpContext
            services.AddHttpContextAccessor();

            // gRPC Server
            services.AddGrpc();

            // Prometheus
            services.AddSystemMetrics();

            // DI-containers
            // Repositories
            services.AddScoped<ITestsRepository, TestsRepository>();
            services.AddScoped<IQuestionsRepository, QuestionsRepository>();
            // Services
            services.AddScoped<ITestsService, AppTestsService>();
            services.AddScoped<IQuestionsService, QuestionsService>();
            // Clients
            services.AddScoped<ILecturesClientForTests, LecturesClientForTests>();
            services.AddScoped<ICoursesClientForTests, CoursesClientForTests>();

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
            // Courses Client
            services.AddGrpcClient<CoursesApi.CoursesApiClient>(o =>
            {
                o.Address = new Uri("https://localhost:3004");
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
