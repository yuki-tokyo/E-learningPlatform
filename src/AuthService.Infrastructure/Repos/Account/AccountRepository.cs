using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces.Account.Repos;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Infrastructure.Repos.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbContextFactory<AuthDb> factory;

        public AccountRepository(IDbContextFactory<AuthDb> factory)
        {
            this.factory = factory;
        }
        public async Task ChangeEmail(string id, string email)
        {
            await using var db = await factory.CreateDbContextAsync();

            await db.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(u => u.Email, email));
        }

        public async Task ChangeName(string id, string name)
        {
            await using var db = await factory.CreateDbContextAsync();

            await db.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(u => u.Name, name));
        }

        public async Task ChangePassword(string id, string password)
        {
            await using var db = await factory.CreateDbContextAsync();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            await db.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(u => u.Password, hashedPassword));
        }

        public async Task<User?> GetById(string id)
        {
            await using var db = await factory.CreateDbContextAsync();

            return await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetMyAccount(string id)
        {
            await using var db = await factory.CreateDbContextAsync();

            return await db.Users
                .FirstAsync(u => u.Id == id);
        }
    }
}
