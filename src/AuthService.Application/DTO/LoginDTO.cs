using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.DTO
{
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
