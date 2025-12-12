using AccountService.Domain.Interfaces;
using AccountService.Infrastructure.Extensions;
using AccountService.Protos;
using EmailService.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AccountService.Infrastructure.gRPC.Clients
{
    public class EmailClientForAccount : IEmailClientForAccount
    {
        private readonly EmailApi.EmailApiClient client;
        private readonly IHttpContextAccessor contextAccessor;

        public EmailClientForAccount(EmailApi.EmailApiClient client, IHttpContextAccessor contextAccessor)
        {
            this.client = client;
            this.contextAccessor = contextAccessor;
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


        public async Task AddVerification(string code, string email)
        {
            try
            {
                var request = new AddVerificationRequest { Useremail = email, Code = code };
                await client.AddVerificationAsync(request);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
