using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Entities
{
    public class Verification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Code { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public  string? UserPassword { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddMinutes(10);
    }
}
