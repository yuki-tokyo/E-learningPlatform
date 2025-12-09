using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddDbContext<DatabaseConnect>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Postgres"),
                    b => b.MigrationsAssembly(typeof(DatabaseConnect).Assembly.FullName)));


            return services;
        }
    }
}
