using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task Register(string name, string email, string pass);
        Task<User?> Login(string email, string pass);
        Task<bool> IsThisEmailRegistered(string email);
    }
}
