using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces
{
    public interface IAuthGrpcClient
    {
        Task<string> Login(string email, string pass);
        Task<string> Register(string email, string name, string pass);
    }
}
