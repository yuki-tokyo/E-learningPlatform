using Common.Exceptions;
using CoursesService.Domain.Interfaces;
using Grpc.Core;
using PaymentService.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Infrastructure.gRPC.Clients
{
    public class PaymentClientForCourses : IPaymentClientForCourses
    {
        private readonly PaymentApi.PaymentApiClient client;

        public PaymentClientForCourses(PaymentApi.PaymentApiClient client)
        {
            this.client = client;
        }

        public async Task DepositMoney(string currentUserId, double amount)
        {
            try
            {
                await client.DepositMoneyAsync(new DepositMoneyRequest { Amount = amount, Id = currentUserId });
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task SpendMoney(string currentUserId, double amount)
        {
            try
            {
                await client.SpendMoneyAsync(new SpendMoneyRequest { Amount = amount, Id = currentUserId });
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                throw new NotEnoughMoneyException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
