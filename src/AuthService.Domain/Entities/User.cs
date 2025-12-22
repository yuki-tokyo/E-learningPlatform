using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Level { get; set; } = 1;
        public int PointsRemainingToNewLevel { get; set; } = 35;
        public int Points { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public double Balance { get; set; } = 0;
        public bool IsEmailVerified { get; set; } = false;
    }
}
