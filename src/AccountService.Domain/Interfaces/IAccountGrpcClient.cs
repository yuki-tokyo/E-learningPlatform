using AccountService.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Domain.Interfaces
{
    public interface IAccountGrpcClient
    {
        Task<UserResponseForClient> GetById(string id);
        Task<UserResponseForClient> GetMyAccount();
        Task ChangeName(string name);
        Task ChangeEmail(string email);
        Task ChangePassword(string password);
    }
}
