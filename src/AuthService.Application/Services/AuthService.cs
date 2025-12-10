using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.Email;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository repos;
        private readonly IJwtService jwt;
        private readonly IEmailVerifyService verify;
        private readonly IVerifyRepository vrepos;
        public AuthService(IAuthRepository repos, 
            IJwtService jwt,
            IEmailVerifyService verify,
            IVerifyRepository vrepos)
        {
            this.repos = repos;
            this.jwt = jwt;
            this.verify = verify;
            this.vrepos = vrepos;
        }
        public async Task<string> Login(string email, string pass)
        {
            var user = await repos.Login(email, pass);
            if (user == null)
            {
                throw new InvalidCredentialsException("Пользователь не найден/данные некорректны.");
            }
            return await jwt.GenerateJwtToken(user);
        }

        public async Task<string> Register(string name, string email, string pass)
        {
            var result = await repos.IsThisEmailRegistered(email);
            if (result)
            {
                throw new UserAlreadyExistsException("Данные уже используются другим пользователем.");
            }
            var code = new Random().Next(100000, 999999).ToString();
            var verif = new Verification { Code = code, UserEmail = email, UserName = name, UserPassword = pass };
            await vrepos.AddVerification(verif);
            await verify.SendVerificationCode(email, code);
            return "Код отправлен вам на почту!";
        }
    }
}
