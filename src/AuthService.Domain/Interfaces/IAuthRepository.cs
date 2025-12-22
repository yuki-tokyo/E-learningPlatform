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
        Task ChangeEmail(string id, string email);
        Task<bool> IsThisEmailRegistered(string email);
        Task<int> EditBalance(string currentUserId, double depositAmount, double spentAmount);
        Task UpdateUserLevel(string userId, int points);
    }
}
