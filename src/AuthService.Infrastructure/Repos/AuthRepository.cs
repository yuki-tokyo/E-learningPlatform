using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Repos
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbContextFactory<DatabaseConnect> factory;

        public AuthRepository(IDbContextFactory<DatabaseConnect> factory)
        {
            this.factory = factory;
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
