using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Data
{
    public class DatabaseConnect : DbContext
    {
        public DatabaseConnect(DbContextOptions<DatabaseConnect> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
