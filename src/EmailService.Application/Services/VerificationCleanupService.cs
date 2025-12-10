using EmailService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Application.Services
{
    public class VerificationCleanupService : BackgroundService
    {
        private readonly ILogger<VerificationCleanupService> logger;
        private readonly IServiceProvider provider;
        private readonly TimeSpan cleanupInterval = TimeSpan.FromMinutes(10);

        public VerificationCleanupService(
            ILogger<VerificationCleanupService> logger,
            IServiceProvider provider)
        {
            this.logger = logger;
            this.provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PerformCleanupAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ошибка чистки истекших кодов.");
                }

                await Task.Delay(cleanupInterval, stoppingToken);
            }
        }

        private async Task PerformCleanupAsync(CancellationToken cancellationToken)
        {
            using var scope = provider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IVerificationCleanupRepository>();

            var deletedCount = await repository.DeleteExpiredVerifications(cancellationToken);

            if (deletedCount > 0)
            {
                logger.LogInformation($"Удалено {deletedCount} истекших кодов.");
            }
            else
            {
                logger.LogDebug("Нет истекших кодов для удаления.");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Сервис чистки кодов остановлен.");
            await base.StopAsync(cancellationToken);
        }
    }
}
