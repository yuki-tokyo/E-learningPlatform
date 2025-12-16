using AuthService.Application.Services;
using AuthService.Application.Services.Account;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.Account.Repos;
using AuthService.Domain.Interfaces.Account.Services;
using AuthService.Domain.Interfaces.gRPC;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.gRPC.Clients;
using AuthService.Infrastructure.Repos;
using AuthService.Infrastructure.Repos.Account;
using AuthService.Protos;
using EmailService.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prometheus.SystemMetrics;
using System;
using System.Collections.Generic;
using System.Text;
using AppAuthService = AuthService.Application.Services.AuthService;

namespace AuthService.Infrastructure.Extensions
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AuthDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(AuthDb).Assembly.FullName)));

            // DbContextFactory
            services.AddDbContextFactory<AuthDb>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(AuthDb).Assembly.FullName)),
                    lifetime: ServiceLifetime.Scoped);

            // HttpContext
            services.AddHttpContextAccessor();

            // gRPC Server
            services.AddGrpc();

            // Prometheus
            services.AddSystemMetrics();

            // DI-containers
            // Repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            // Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AppAuthService>();
            services.AddScoped<IAccountService, AccountService>();
            // Clients
            services.AddScoped<IEmailClientForAuth, EmailClientForAuth>();

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
            // Email client
            services.AddGrpcClient<EmailApi.EmailApiClient>(o =>
            {
                o.Address = new Uri("https://localhost:3002");
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
