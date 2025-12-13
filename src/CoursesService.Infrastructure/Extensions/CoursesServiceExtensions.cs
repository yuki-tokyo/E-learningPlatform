using CoursesService.Application.Services;
using CoursesService.Domain.Interfaces;
using CoursesService.Infrastructure.Data;
using CoursesService.Infrastructure.gRPC.Clients;
using CoursesService.Infrastructure.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AppCoursesService = CoursesService.Application.Services.CoursesService;
using System;
using System.Collections.Generic;
using System.Text;
using PaymentService.Protos;

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

            // DI-containers
            // Repositories
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            // Services
            services.AddScoped<ICoursesService, AppCoursesService>();
            // Clients
            services.AddScoped<IPaymentClientForCourses, PaymentClientForCourses>();

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

            return services;
        }
    }
}
