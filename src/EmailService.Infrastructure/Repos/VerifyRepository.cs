using EmailService.Domain.Entities;
using EmailService.Domain.Interfaces;
using EmailService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Infrastructure.Repos
{
    public class VerifyRepository : IVerifyRepository
    {
        private readonly IDbContextFactory<EmailDb> factory;

        public VerifyRepository(IDbContextFactory<EmailDb> factory)
        {
            this.factory = factory;
        }

        public async Task AddVerification(Verification verif)
        {
            await using var db = await factory.CreateDbContextAsync();

            await db.Verifications.AddAsync(verif);
            await db.SaveChangesAsync();
        }

        public async Task DeleteVerification(string email)
        {
            await using var db = await factory.CreateDbContextAsync();

            await db.Verifications
                .Where(v => v.UserEmail == email)
                .ExecuteDeleteAsync();

            await db.SaveChangesAsync();
        }

        public async Task<Verification?> FindVerification(string email)
        {
            await using var db = await factory.CreateDbContextAsync();

            var verif = await db.Verifications
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.UserEmail == email);

            return verif;
        }
    }
}
