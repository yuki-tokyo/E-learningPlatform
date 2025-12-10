using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Services.Email
{
    public class VerifyService : IVerifyService
    {
        private readonly IVerifyRepository vrepos;
        private readonly IAuthRepository repos;
        public VerifyService(IVerifyRepository vrepos, IAuthRepository repos)
        {
            this.vrepos = vrepos;
            this.repos = repos;
        }

        public async Task<string> VerifyEmail(string email, string code)
        {
            var verif = await vrepos.FindVerification(email);
            if (verif == null)
            {
                throw new VerificationException("Данные для верификации почты некорректны");
            }
            else if (verif.ExpirationDate < DateTime.UtcNow)
            {
                throw new VerificationException("Код просрочен, запросите новый.");
            }
            else if (verif.Code != code)
            {
                throw new VerificationException("Неверный код.");
            }
            await repos.Register(verif.UserName, email, verif.UserPassword);
            return "Почта успешно подтверждена!";
        }
    }
}
