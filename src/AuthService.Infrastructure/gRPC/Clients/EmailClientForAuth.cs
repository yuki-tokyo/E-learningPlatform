using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces.gRPC;
using AuthService.Protos;
using EmailService.Protos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.gRPC.Clients
{
    public class EmailClientForAuth : IEmailClientForAuth
    {
        private readonly EmailApi.EmailApiClient client;

        public EmailClientForAuth(EmailApi.EmailApiClient client)
        {
            this.client = client;
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
                throw new InvalidCredentialsException(ex.Status.Detail);
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
                throw new InvalidCredentialsException(ex.Status.Detail);
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
    }
}
