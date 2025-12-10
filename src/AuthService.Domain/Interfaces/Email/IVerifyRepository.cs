using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces.Email
{
    public interface IVerifyRepository
    {
        Task AddVerification(Verification verif);
        Task<Verification?> FindVerification(string email);
    }
}
