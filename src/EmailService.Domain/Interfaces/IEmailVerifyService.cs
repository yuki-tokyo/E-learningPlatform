using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Interfaces
{
    public interface IEmailVerifyService
    {
        Task SendVerificationCode(string email, string code);
    }
}
