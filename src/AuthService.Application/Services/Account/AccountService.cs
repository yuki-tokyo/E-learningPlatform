using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.Account.Repos;
using AuthService.Domain.Interfaces.Account.Services;
using AuthService.Domain.Interfaces.gRPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IEmailClientForAuth emailClient;
        private readonly IAccountRepository repos;
        private readonly IAuthRepository authRepos;
        public AccountService
            (IEmailClientForAuth emailClient, 
            IAccountRepository repos, 
            IAuthRepository authRepos)
        {
            this.emailClient = emailClient;
            this.repos = repos;
            this.authRepos = authRepos;
        }
        public async Task ChangeEmail(string id, string email)
        {
            var result = await authRepos.IsThisEmailRegistered(email);
            if (result)
            {
                throw new UserAlreadyExistsException("Данный email уже занят.");
            }
            var code = new Random().Next(100000, 999999).ToString();
            var msg = await emailClient.SendCode(email, code);
            await emailClient.AddVerificationForChangedEmail(id, code, email);
        }

        public async Task VerifyChangedEmail(string email, string code)
        {
            await emailClient.VerifyChangedEmail(email, code);
        }

        public async Task ChangeName(string id, string name)
        {
            await repos.ChangeName(id, name);
        }

        public async Task ChangePassword(string id, string password)
        {
            await repos.ChangePassword(id, password);
        }

        public async Task<User> GetById(string id)
        {
            var user =  await repos.GetById(id);
            if (user == null)
            {
                throw new UserNotFoundException("Пользователь не найден.");
            }
            return user;
        }

        public async Task<User> GetMyAccount(string id)
        {
            return await repos.GetMyAccount(id);
        }
    }
}
