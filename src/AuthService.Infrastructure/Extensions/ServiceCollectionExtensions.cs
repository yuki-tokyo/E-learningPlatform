using AuthService.Application.Services;
using AuthService.Application.Services.Email;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.Email;
using AuthService.Domain.Interfaces.gRPC;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.gRPC.Clients;
using AuthService.Infrastructure.Repos;
using AuthService.Protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AppAuthService = AuthService.Application.Services.AuthService;

namespace AuthService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<DatabaseConnect>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(DatabaseConnect).Assembly.FullName)));

            // DbContextFactory
            services.AddDbContextFactory<DatabaseConnect>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(DatabaseConnect).Assembly.FullName)),
                    lifetime: ServiceLifetime.Scoped);

            // gRPC Client
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
            // DI for client
            services.AddScoped<IAuthGrpcClient, AuthGrpcClient>();

            // DI-containers
            // Repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IVerifyRepository, VerifyRepository>();
            services.AddScoped<IVerificationCleanupRepository, VerificationCleanupRepository>();
            //Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AppAuthService>();
            services.AddScoped<IEmailVerifyService, EmailVerifyService>();
            services.AddScoped<IVerifyService, VerifyService>();
            services.AddHostedService<VerificationCleanupService>();

            return services;
        }
    }
}
