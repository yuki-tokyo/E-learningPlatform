using AuthService.Protos;
using Common.Exceptions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using PaymentService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace PaymentService.Infrastructure.gRPC.Clients
{
    public class AuthClientForPayment : IAuthClientForPayment
    {
        private readonly AuthApi.AuthApiClient client;

        public AuthClientForPayment(AuthApi.AuthApiClient client)
        {
            this.client = client;
        }
        public async Task DepositMoney(string userId, double amount)
        {
            try
            {
                await client.EditBalanceAsync(new EditBalanceRequest { UserId = userId, DepositAmount = amount });
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

        public async Task SpendMoney(string userId, double amount)
        {
            try
            {
                await client.EditBalanceAsync(new EditBalanceRequest { UserId = userId, SpentAmount = amount });
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
