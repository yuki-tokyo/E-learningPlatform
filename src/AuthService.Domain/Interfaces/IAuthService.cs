using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(string name, string email, string pass);
        Task<string> Login(string email, string pass);
        Task AddUser(string name, string email, string pass);
        Task ChangeEmail(string id, string email);
        Task<bool> IsThisEmailRegistered(string email);
    }
}
