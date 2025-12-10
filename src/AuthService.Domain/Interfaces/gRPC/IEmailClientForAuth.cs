using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces.gRPC
{
    public interface IEmailClientForAuth
    {
        Task<string> SendCode(string email, string code);
        Task<string> VerifyEmail(string email, string code);
        Task AddVerification(string code, string name, string email, string pass);
    }
}
