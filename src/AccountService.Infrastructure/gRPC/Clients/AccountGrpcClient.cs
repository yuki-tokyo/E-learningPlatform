using AccountService.Domain.Exceptions;
using AccountService.Domain.Interfaces;
using AccountService.Domain.Responses;
using AccountService.Infrastructure.Extensions;
using AccountService.Protos;
using Common.Extensions;
using EmailService.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Security;

namespace AccountService.Infrastructure.gRPC.Clients
{
    public class AccountGrpcClient : IAccountGrpcClient
    {
        private readonly AccountApi.AccountApiClient client;
        private readonly IEmailClientForAccount emailClient;
        private readonly IHttpContextAccessor contextAccessor;

        public AccountGrpcClient
            (AccountApi.AccountApiClient client, 
            IHttpContextAccessor contextAccessor,
            IEmailClientForAccount emailClient)
        {
            this.client = client;
            this.contextAccessor = contextAccessor;
            this.emailClient = emailClient;
        }

        public async Task ChangeEmail(string email)
        {
            try
            {
                var headers = contextAccessor.GetAuthMetadata();

                var response = await client.ChangeEmailAsync(new ChangeEmailRequest { Email = email }, headers: headers);
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

        public async Task VerifyChangedEmail(string email, string code)
        {
            try
            {
                var headers = contextAccessor.GetAuthMetadata();

                await emailClient.VerifyChangedEmail(email, code);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task ChangeName(string name)
        {
            try
            {
                var headers = contextAccessor.GetAuthMetadata();

                var response = await client.ChangeNameAsync(new ChangeNameRequest { Name = name }, headers: headers);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task ChangePassword(string password)
        {
            try
            {
                var headers = contextAccessor.GetAuthMetadata();

                var response = await client.ChangePasswordAsync(new ChangePasswordRequest { Password = password }, headers: headers);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task<UserResponseForClient> GetById(string id)
        {
            try
            {
                var response = await client.GetByIdAsync(new GetByIdRequest { Id = id });
                return new UserResponseForClient { Email = response.Email , Name = response.Name };
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new UserNotFoundException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task<UserResponseForClient> GetMyAccount()
        {
            try
            {
                var headers = contextAccessor.GetAuthMetadata();

                var response = await client.GetMyAccountAsync(new GetMyAccountRequest { } , headers: headers);
                return new UserResponseForClient { Email = response.Email, Name = response.Name, Password = response.Password};
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
