using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces.Email
{
    public interface IEmailVerifyService
    {
        Task SendVerificationCode(string email, string code);
    }
}
