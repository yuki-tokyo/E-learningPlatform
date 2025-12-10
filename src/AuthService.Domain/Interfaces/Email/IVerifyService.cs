using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces.Email
{
    public interface IVerifyService
    {
        Task<string> VerifyEmail(string email, string code);
    }
}
