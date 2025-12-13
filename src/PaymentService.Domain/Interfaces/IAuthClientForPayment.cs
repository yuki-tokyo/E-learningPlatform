using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain.Interfaces
{
    public interface IAuthClientForPayment
    {
        Task DepositMoney(string userId, double amount);
        Task SpendMoney(string userId, double amount);
    }
}
