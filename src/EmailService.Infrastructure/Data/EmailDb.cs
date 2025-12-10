using EmailService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Infrastructure.Data
{
    public class EmailDb : DbContext
    {
        public EmailDb(DbContextOptions<EmailDb> options) : base(options) { }

        public DbSet<Verification> Verifications { get; set; }
    }
}
