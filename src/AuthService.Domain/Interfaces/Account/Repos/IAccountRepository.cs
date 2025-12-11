using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces.Account.Repos
{
    public interface IAccountRepository
    {
        Task<User?> GetById(string id);
        Task<User> GetMyAccount(string id);
        Task ChangeName(string id, string name);
        Task ChangeEmail(string id, string email);
        Task ChangePassword(string id, string password);
    }
}
