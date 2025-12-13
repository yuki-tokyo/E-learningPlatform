using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Infrastructure.Repos
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbContextFactory<AuthDb> factory;

        public AuthRepository(IDbContextFactory<AuthDb> factory)
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

        public async Task<int> EditBalance(string currentUserId, double depositAmount, double spentAmount)
        {
            await using var db = await factory.CreateDbContextAsync();

            var user = await db.Users
                .FirstAsync(u => u.Id == currentUserId);

            user.Balance += depositAmount;
            if (user.Balance < spentAmount)
            {
                return 0;
            }

            user.Balance -= spentAmount;
            await db.SaveChangesAsync();

            return 1;
        }

        public async Task<bool> IsThisEmailRegistered(string email)
        {
            await using var db = await factory.CreateDbContextAsync();

            try
            {
                var user = await db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email);

                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка проверки почты: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> Login(string email, string pass)
        {
            await using var db = await factory.CreateDbContextAsync();

            var user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(pass, user.Password);
            return isPasswordValid ? user : null;
        }

        public async Task Register(string name, string email, string pass)
        {
            await using var db = await factory.CreateDbContextAsync();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(pass);
            var user = new User { Name = name, Email = email, Password = hashedPassword , IsEmailVerified = true};

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
    }
}
