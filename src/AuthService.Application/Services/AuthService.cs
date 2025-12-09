using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository repos;
        private readonly IJwtService jwt;
        public AuthService(IAuthRepository repos, IJwtService jwt)
        {
            this.repos = repos;
            this.jwt = jwt;
        }
        public async Task<string> Login(string email, string pass)
        {
            var user = await repos.Login(email, pass);
            if(user == null)
            {
                throw new InvalidCredentialsException("Пользователь не найден/данные некорректны.");
            }
            return await jwt.GenerateJwtToken(user);
        }

        public async Task<string> Register(string name, string email, string pass)
        {
            var result = await repos.IsThisEmailRegistered(email);
            if (result)
            {
                throw new UserAlreadyExistsException("Данные уже используются другим пользователем.");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(pass);
            var user = await repos.Register(name, email, hashedPassword);
            return await jwt.GenerateJwtToken(user);
        }
    }
}
