using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces.Account.Repos;
using AuthService.Domain.Interfaces.Account.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository repos;
        public AccountService(IAccountRepository repos)
        {
            this.repos = repos;
        }
        public async Task ChangeEmail(string id, string email)
        {
            await repos.ChangeEmail(id, email);
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
