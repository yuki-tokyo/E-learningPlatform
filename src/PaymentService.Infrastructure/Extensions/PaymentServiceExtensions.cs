using AuthService.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.gRPC.Clients;
using Prometheus.SystemMetrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Infrastructure.Extensions
{
    public static class PaymentServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // gRPC Server
            services.AddGrpc();

            // Prometheus
            services.AddSystemMetrics();

            // DI-containers
            // Clients
            services.AddScoped<IAuthClientForPayment, AuthClientForPayment>();

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
