using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AuthService.Protos;
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

        public override async Task<JwtTokenResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            try
            {
                var result = await service.Register(request.Name, request.Email, request.Password);

                return new JwtTokenResponse
                {
                    Token = result
                };
            }
            catch (UserAlreadyExistsException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
        }
    }
}
