using AuthService.Protos;
using Grpc.Core;
using ProgressService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgressService.Infrastructure.gRPC.Clients
{
    public class AuthClientForProgress : IAuthClientForProgress
    {
        private readonly AuthApi.AuthApiClient client;

        public AuthClientForProgress(AuthApi.AuthApiClient client)
        {
            this.client = client;
        }
        public async Task UpdateUserLevel(string userId, int points)
        {
            try
            {
                await client.UpdateUserLevelAsync(new UpdateUserLevelRequest { UserId = userId, Points = points });
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
