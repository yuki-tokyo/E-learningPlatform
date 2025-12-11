using AccountService.Protos;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces.Account.Services;
using AuthService.Infrastructure.Extensions.Account;
using Grpc.Core;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.API.gRPC.Services
{
    public class AccountGrpcServer : AccountApi.AccountApiBase
    {
        private readonly IAccountService service;
        public AccountGrpcServer(IAccountService service)
        {
            this.service = service;
        }

        public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
        {
            try
            {
                var result = await service.GetById(request.Id);

                return new GetByIdResponse
                {
                    Id = request.Id,
                    Email = result.Email,
                    Name = result.Name
                };
            }
            catch (UserNotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
        }

        public override async Task<GetMyAccountResponse> GetMyAccount(GetMyAccountRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();

            var result = await service.GetMyAccount(userId);

            return new GetMyAccountResponse
            {
                Id = userId,
                Email = result.Email,
                Name = result.Name,
                Password = result.Password
            };
        }

        public override async Task<ChangeResponse> ChangeName(ChangeNameRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();

            await service.ChangeName(userId, request.Name);

            return new ChangeResponse { };
        }
        public override async Task<ChangeResponse> ChangeEmail(ChangeEmailRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();

            await service.ChangeEmail(userId, request.Email);

            return new ChangeResponse { };
        }
        public override async Task<ChangeResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();

            await service.ChangePassword(userId, request.Password);

            return new ChangeResponse { };
        }


    }
}
