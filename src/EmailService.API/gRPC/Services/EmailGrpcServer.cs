using Common.Exceptions;
using Common.Extensions;
using EmailService.Domain.Entities;
using EmailService.Domain.Interfaces;
using EmailService.Infrastructure.Extensions;
using EmailService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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

        public override async Task<Empty> VerifyChangedEmail(VerifyChangedEmailRequest request, ServerCallContext context)
        {
            try
            {
                var userId = context.GetUserId();

                await vservice.VerifyChangedEmail(userId, request.Email, request.Code);

                return new Empty();
            }
            catch (VerificationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }


        public override async Task<Empty> AddVerification(AddVerificationRequest request, ServerCallContext context)
        {
            var verif = new Verification
            {
                UserId = request.Userid,
                UserEmail = request.Useremail,
                UserName = request.Username,
                UserPassword = request.Userpassword,
                Code = request.Code
            };

            await vservice.AddVerification(verif);

            return new Empty();
        }
    }
}
