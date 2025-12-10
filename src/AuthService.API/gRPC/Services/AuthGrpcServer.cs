using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.Email;
using AuthService.Protos;
using Grpc.Core;
using static AuthService.Protos.AuthApi;

namespace AuthService.API.gRPC.Services
{
    public class AuthGrpcServer : AuthApiBase
    {
        private readonly IAuthService service;
        private readonly IVerifyService vservice;
        public AuthGrpcServer(IAuthService service, IVerifyService vservice)
        {
            this.service = service;
            this.vservice = vservice;
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
        }

        public override async Task<VerifyResponse> Verify(VerifyRequest request, ServerCallContext context)
        {
            try
            {
                var result = await vservice.VerifyEmail(request.Email, request.Code);

                return new VerifyResponse
                {
                    Msg = result
                };
            }
            catch (VerificationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }
    }
}
