using EmailService.Domain.Entities;
using EmailService.Domain.Exceptions;
using EmailService.Domain.Interfaces;
using EmailService.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Authentication;
using static EmailService.Protos.EmailApi;

namespace EmailService.API.gRPC.Services
{
    public class EmailGrpcServer : EmailApiBase
    {
        private readonly IEmailVerifyService service;
        private readonly IVerifyService vservice;
        public EmailGrpcServer(IEmailVerifyService service, IVerifyService vservice)
        {
            this.service = service;
            this.vservice = vservice;
        }
        public override async Task<SendCodeResponse> SendCode(SendCodeRequest request, ServerCallContext context)
        {
            try
            {
                await service.SendVerificationCode(request.Email, request.Code);

                return new SendCodeResponse
                {
                    Msg = "Код отправлен на почту!"
                };
            }
            catch (VerificationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }

        public override async Task<VerifyEmailResponse> VerifyEmail(VerifyEmailRequest request, ServerCallContext context)
        {
            try
            {
                await vservice.VerifyEmail(request.Email, request.Code);

                return new VerifyEmailResponse
                {
                    Msg = "Почта успешно подтверждена!"
                };
            }
            catch (VerificationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }


        public override async Task<AddVerificationResponse> AddVerification(AddVerificationRequest request, ServerCallContext context)
        {
            var verif = new Verification
            { Code = request.Code, UserEmail = request.Useremail, UserName = request.Username, UserPassword = request.Userpassword };
            await vservice.AddVerification(verif);

            return new AddVerificationResponse { };
        }
    }
}
