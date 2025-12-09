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
        private readonly DatabaseConnect db;
        private readonly IJwtService jwt;
        public AuthRepository(DatabaseConnect db, IJwtService jwt)
        {
            this.db = db;
            this.jwt = jwt;
        }

        public async Task<bool> IsThisEmailRegistered(string email)
        {
            try
            {
                var user = await db.Users
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
            var user = await db
                .Users
                .FirstOrDefaultAsync
                (u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(pass, user.Password);

            return isPasswordValid ? user : null;
        }

        public async Task<User> Register(string name, string email, string pass)
        {
            var user = new User { Name = name, Email = email, Password = pass };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
    }
}
