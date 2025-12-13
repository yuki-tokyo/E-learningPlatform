using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces
{
    public interface IPaymentClientForCourses
    {
        Task DepositMoney(string currentUserId, double amount);
        Task SpendMoney(string currentUserId, double amount);
    }
}
