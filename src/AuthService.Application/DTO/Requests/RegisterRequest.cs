using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.DTO.Requests
{
    public class RegisterRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
