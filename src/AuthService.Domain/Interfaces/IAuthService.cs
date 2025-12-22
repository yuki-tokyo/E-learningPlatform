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
        Task EditBalance(string currentUserId, double depositAmount, double spentAmount);
        Task UpdateUserLevel(string userId, int points);
    }
}
