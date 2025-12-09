using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AuthService.Protos;
using Grpc.Core;

namespace AuthService.API.gRPC.Services
{
    public class AuthGrpcClient : IAuthGrpcClient
    {
        private readonly AuthApi.AuthApiClient client;

        public AuthGrpcClient(AuthApi.AuthApiClient client)
        {
            this.client = client;
        }

        public async Task<string> Login(string email, string pass)
        {
            try
            {
                var request = new LoginRequest { Email = email, Password = pass };
                var response = await client.LoginAsync(request);

                return response.Token;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unauthenticated)
            {
                throw new InvalidCredentialsException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task<string> Register(string email, string name, string pass)
        {
            try
            {
                var request = new RegisterRequest { Email = email, Password = pass, Name = name };
                var response = await client.RegisterAsync(request);

                return response.Token;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                throw new UserAlreadyExistsException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
