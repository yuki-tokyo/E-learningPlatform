using Common.Kafka.Settings;
using CoursesService.Protos;
using LecturesService.Domain.Interfaces;
using LecturesService.Infrastructure.Data;
using LecturesService.Infrastructure.gRPC.Clients;
using LecturesService.Infrastructure.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prometheus.SystemMetrics;
using System;
using System.Collections.Generic;
using System.Text;
using AppLecturesService = LecturesService.Application.Services.LecturesService;

namespace LecturesService.Infrastructure.Extentions
{
    public static class LecturesServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<LecturesDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(LecturesDb).Assembly.FullName)));

            // DbContextFactory
            services.AddDbContextFactory<LecturesDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(LecturesDb).Assembly.FullName)),
                    lifetime: ServiceLifetime.Scoped);

            // HttpContext
            services.AddHttpContextAccessor();

            // gRPC Server
            services.AddGrpc();

            // Prometheus
            services.AddSystemMetrics();

            // DI-containers
            // Repositories
            services.AddScoped<ILecturesRepository, LecturesRepository>();
            // Services
            services.AddScoped<ILecturesService, AppLecturesService>();
            // Clients
            services.AddScoped<ICoursesClientForLectures, CoursesClientForLectures>();

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
