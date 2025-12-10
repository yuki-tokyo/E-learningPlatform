using EmailService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Interfaces
{
    public interface IVerifyRepository
    {
        Task AddVerification(Verification verif);
        Task<Verification?> FindVerification(string email);
    }
}
