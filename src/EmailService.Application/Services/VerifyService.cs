using Common.Exceptions;
using EmailService.Domain.Entities;
using EmailService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Application.Services
{
    public class VerifyService : IVerifyService
    {
        private readonly IVerifyRepository vrepos;
        private readonly IAuthClientForEmail authClient;
        public VerifyService(IVerifyRepository vrepos, IAuthClientForEmail authClient)
        {
            this.vrepos = vrepos;
            this.authClient = authClient;
        }

        public async Task<string> VerifyEmail(string email, string code)
        {
            var verif = await vrepos.FindVerification(email);
            if (verif == null)
            {
                throw new VerificationException("Данные для верификации почты некорректны.");
            }
            else if (verif.ExpirationDate < DateTime.UtcNow)
            {
                throw new VerificationException("Код просрочен, запросите новый.");
            }
            else if (verif.Code != code)
            {
                throw new VerificationException("Неверный код.");
            }

            await authClient.AddUser(verif.UserName, email, verif.UserPassword);
            await vrepos.DeleteVerification(email);

            return "Почта успешно подтверждена!";
        }

        public async Task VerifyChangedEmail(string currentUserId, string email, string code)
        {
            var verif = await vrepos.FindVerification(email);

            if (verif == null)
            {
                throw new VerificationException("Данные для верификации почты некорректны.");
            }
            else if (verif.UserId != currentUserId)
            {
                throw new VerificationException("Заявка на смену почты была подана не вами.");
            }
            else if (verif.ExpirationDate < DateTime.UtcNow)
            {
                throw new VerificationException("Код просрочен, запросите новый.");
            }
            else if (verif.Code != code)
            {
                throw new VerificationException("Неверный код.");
            }

            await authClient.ChangeEmail(email);
            await vrepos.DeleteVerification(email);
        }

        public async Task AddVerification(Verification verif)
        {
            await vrepos.AddVerification(verif);
        }
    }
}
