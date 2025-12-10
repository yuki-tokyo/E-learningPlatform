using AuthService.Protos;
using EmailService.Application.Services;
using EmailService.Domain.Interfaces;
using EmailService.Infrastructure.Data;
using EmailService.Infrastructure.gRPC.Clients;
using EmailService.Infrastructure.Repos;
using EmailService.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Infrastructure.Extensions
{
    public static class EmailServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<EmailDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(EmailDb).Assembly.FullName)));

            // DbContextFactory
            services.AddDbContextFactory<EmailDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(EmailDb).Assembly.FullName)),
                    lifetime: ServiceLifetime.Scoped);

            // gRPC Server
            services.AddGrpc();

            // DI-containers
            // Repositories
            services.AddScoped<IVerifyRepository, VerifyRepository>();
            services.AddScoped<IVerificationCleanupRepository, VerificationCleanupRepository>();
            // Services
            services.AddScoped<IEmailVerifyService, EmailVerifyService>();
            services.AddScoped<IVerifyService, VerifyService>();
            services.AddScoped<IAuthClientForEmail, AuthClientForEmail>();
            services.AddHostedService<VerificationCleanupService>();

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
