using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsEmailVerified { get; set; } = false;
    }
}
