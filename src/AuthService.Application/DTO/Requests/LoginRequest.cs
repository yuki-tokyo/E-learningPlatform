using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.DTO.Requests
{
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
