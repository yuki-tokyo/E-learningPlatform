using AuthService.Protos;
using EmailService.Domain.Interfaces;
using EmailService.Infrastructure.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace EmailService.Infrastructure.gRPC.Clients
{
    public class AuthClientForEmail : IAuthClientForEmail
    {
        private readonly AuthApi.AuthApiClient client;
        private readonly IHttpContextAccessor contextAccessor;

        public AuthClientForEmail(AuthApi.AuthApiClient client, IHttpContextAccessor contextAccessor)
        {
            this.client = client;
            this.contextAccessor = contextAccessor;
        }

        public async Task AddUser(string? name, string email, string? pass)
        {
            try
            {
                var request = new AddUserRequest { Email = email, Name = name, Password = pass };
                var response = await client.AddUserAsync(request);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
        public async Task ChangeEmail(string email)
        {
            try
            {

                var headers = contextAccessor.GetAuthMetadata();

                var request = new ChangeEmailRequest { Email = email };
                await client.ChangeEmailAsync(request, headers: headers);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
