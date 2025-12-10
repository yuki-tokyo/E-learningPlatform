using EmailService.Domain.Interfaces;
using EmailService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Infrastructure.Repos
{
    public class VerificationCleanupRepository : IVerificationCleanupRepository
    {
        private readonly IDbContextFactory<EmailDb> factory;

        public VerificationCleanupRepository(IDbContextFactory<EmailDb> factory)
        {
            this.factory = factory;
        }

        public async Task<int> DeleteExpiredVerifications(CancellationToken cancellationToken)
        {
            await using var db = await factory.CreateDbContextAsync(cancellationToken);

            var expiredVerifications = await db.Verifications
                .Where(v => v.ExpirationDate < DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            if (!expiredVerifications.Any())
                return 0;

            db.Verifications.RemoveRange(expiredVerifications);
            var deletedCount = await db.SaveChangesAsync(cancellationToken);

            return deletedCount;
        }
    }
}
