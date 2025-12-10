using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Data
{
    public class AuthDb : DbContext
    {
        public AuthDb(DbContextOptions<AuthDb> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
