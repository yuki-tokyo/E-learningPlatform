using AuthService.Application.Services;
using AuthService.Application.Services.Email;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.Email;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppAuthService = AuthService.Application.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Text;

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

            // DI-containers
            // Repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IVerifyRepository, VerifyRepository>();
            //Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AppAuthService>();
            services.AddScoped<IEmailVerifyService, EmailVerifyService>();
            services.AddScoped<IVerifyService, VerifyService>();

            return services;
        }
    }
}
