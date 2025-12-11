using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Domain.Responses
{
    public class UserResponseForClient
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
    }
}
