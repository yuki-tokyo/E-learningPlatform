using AuthService.Domain.Interfaces.Email;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Repos
{
    public class VerificationCleanupRepository : IVerificationCleanupRepository
    {
        private readonly IDbContextFactory<DatabaseConnect> factory;

        public VerificationCleanupRepository(IDbContextFactory<DatabaseConnect> factory)
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
