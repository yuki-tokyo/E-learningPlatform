using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.gRPC;
using Common.Exceptions;
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
        private readonly IEmailClientForAuth emailClient;
        public AuthService(IAuthRepository repos, 
            IJwtService jwt,
            IEmailClientForAuth emailClient)
        {
            this.repos = repos;
            this.jwt = jwt;
            this.emailClient = emailClient;
        }

        public async Task AddUser(string name, string email, string pass)
        {
            await repos.Register(name, email, pass);
        }

        public async Task ChangeEmail(string id, string email)
        {
            await repos.ChangeEmail(id, email);
        }

        public async Task EditBalance(string currentUserId, double depositAmount, double spentAmount)
        {
            var result = await repos.EditBalance(currentUserId, depositAmount, spentAmount);

            if(result == 0)
            {
                throw new NotEnoughMoneyException("На счету недостаточно средств.");
            }
        }

        public async Task<bool> IsThisEmailRegistered(string email)
        {
            return await repos.IsThisEmailRegistered(email);
        }

        public async Task<string> Login(string email, string pass)
        {
            var user = await repos.Login(email, pass);
            if (user == null)
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
                throw new UserAlreadyExistsException("Данный email уже занят.");
            }

            var code = new Random().Next(100000, 999999).ToString();
            var msg = await emailClient.SendCode(email, code);
            await emailClient.AddVerification(code, name, email, pass);

            return msg;
        }

        public async Task UpdateUserLevel(string userId, int points)
        {
            await repos.UpdateUserLevel(userId, points);
        }
    }
}
