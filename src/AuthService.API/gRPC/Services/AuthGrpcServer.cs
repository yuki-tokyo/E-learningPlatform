using AuthService.Domain.Exceptions;
using AuthService.Domain.Exceptions.Email;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Extensions.Account;
using AuthService.Protos;
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
        public override async Task<JwtTokenResponse> Login(LoginRequest request, ServerCallContext context)
        {
            try
            {
                var result = await service.Login(request.Email, request.Password);

                return new JwtTokenResponse
                {
                    Token = result
                };
            }
            catch (InvalidCredentialsException ex)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
            }
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            try
            {
                var result = await service.Register(request.Name, request.Email, request.Password);

                return new RegisterResponse
                {
                    Msg = result
                };
            }
            catch (UserAlreadyExistsException ex)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
            catch (VerificationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
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
    }
}
