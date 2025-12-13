using Common.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PaymentService.Domain.Interfaces;
using PaymentService.Protos;

namespace PaymentService.API.gRPC.Services
{
    public class PaymentGrpcServer : PaymentApi.PaymentApiBase
    {
        private readonly IAuthClientForPayment authClient;
        public PaymentGrpcServer(IAuthClientForPayment authClient)
        {
            this.authClient = authClient;
        }

        public override async Task<Empty> DepositMoney(DepositMoneyRequest request, ServerCallContext context)
        {
            await authClient.DepositMoney(request.Id, request.Amount);

            return new Empty();
        }

        public override async Task<Empty> SpendMoney(SpendMoneyRequest request, ServerCallContext context)
        {
            try
            {
                await authClient.SpendMoney(request.Id, request.Amount);

                return new Empty();
            }
            catch (NotEnoughMoneyException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }
    }
}
