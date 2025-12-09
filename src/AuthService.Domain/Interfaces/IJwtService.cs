using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
