using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Domain.Interfaces
{
    public interface IEmailClientForAccount
    {
        Task VerifyChangedEmail(string email, string code);
        Task AddVerification(string code, string email);
        Task<string> SendCode(string email, string code);
    }
}
