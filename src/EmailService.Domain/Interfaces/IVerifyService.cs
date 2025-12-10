using EmailService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Interfaces
{
    public interface IVerifyService
    {
        Task<string> VerifyEmail(string email, string code);
        Task AddVerification(Verification verif);
    }
}
