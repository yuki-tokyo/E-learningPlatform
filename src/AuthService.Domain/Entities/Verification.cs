using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Entities
{
    public class Verification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Code { get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPassword { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddMinutes(10);
    }
}
