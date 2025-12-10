using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Interfaces
{
    public interface IAuthClientForEmail
    {
        Task AddUser(string name, string email, string pass);
    }
}
