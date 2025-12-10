using AuthService.Protos;
using EmailService.Domain.Interfaces;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace EmailService.Infrastructure.gRPC.Clients
{
    public class AuthClientForEmail : IAuthClientForEmail
    {
        private readonly AuthApi.AuthApiClient client;

        public AuthClientForEmail(AuthApi.AuthApiClient client)
        {
            this.client = client;
        }

        public async Task AddUser(string name, string email, string pass)
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
    }
}
