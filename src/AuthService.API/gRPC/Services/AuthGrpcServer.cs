using AuthService.Domain.Interfaces;
using AuthService.Protos;
using Common.Exceptions;
using Common.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static AuthService.Protos.AuthApi;

namespace AuthService.API.gRPC.Services
{
    public class AuthGrpcServer : AuthApiBase
    {
        private readonly IAuthService service;
        public AuthGrpcServer(IAuthService service)
        {
            this.service = service;
        }

        public override async Task<Empty> AddUser(AddUserRequest request, ServerCallContext context)
        {
            await service.AddUser(request.Name, request.Email, request.Password);

            return new Empty();
        }
        
        public override async Task<Empty> ChangeEmail(ChangeEmailRequest request, ServerCallContext context)
        {
            var userId = context.GetUserId();

            await service.ChangeEmail(userId, request.Email);

            return new Empty();
        }

        public override async Task<Empty> EditBalance(EditBalanceRequest request, ServerCallContext context)
        {
            try
            {
                await service.EditBalance(request.UserId, request.DepositAmount, request.SpentAmount);

                return new Empty();
            }
            catch (NotEnoughMoneyException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }
    }
}
