using AuthService.Domain.Exceptions;
using AuthService.Domain.Exceptions.Email;
using AuthService.Domain.Interfaces.gRPC;
using AuthService.Infrastructure.Extensions.Account;
using EmailService.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.gRPC.Clients
{
    public class EmailClientForAuth : IEmailClientForAuth
    {
        private readonly EmailApi.EmailApiClient client;
        private readonly IHttpContextAccessor contextAccessor;

        public EmailClientForAuth(EmailApi.EmailApiClient client, IHttpContextAccessor contextAccessor)
        {
            this.client = client;
            this.contextAccessor = contextAccessor;
        }
        public async Task<string> SendCode(string email, string code)
        {
            try
            {
                var request = new SendCodeRequest { Email = email, Code = code };
                var response = await client.SendCodeAsync(request);

                return response.Msg;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                throw new VerificationException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task<string> VerifyEmail(string email, string code)
        {
            try
            {
                var request = new VerifyEmailRequest { Email = email, Code = code };
                var response = await client.VerifyEmailAsync(request);

                return response.Msg;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                throw new VerificationException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task AddVerification(string code, string name, string email, string pass)
        {
            try
            {
                var request = new AddVerificationRequest { Useremail = email, Username = name, Userpassword = pass, Code = code };
                await client.AddVerificationAsync(request);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task AddVerificationForChangedEmail(string id, string code, string email)
        {
            try
            {
                var request = new AddVerificationRequest { Userid = id, Useremail = email, Code = code };
                await client.AddVerificationAsync(request);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task VerifyChangedEmail(string email, string code)
        {
            try
            {
                var headers = contextAccessor.GetAuthMetadata();

                await client.VerifyChangedEmailAsync(new VerifyChangedEmailRequest { Email = email, Code = code }, headers: headers);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
