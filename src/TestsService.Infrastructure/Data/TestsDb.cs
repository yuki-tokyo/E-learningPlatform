using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Domain.Entities;

namespace TestsService.Infrastructure.Data
{
    public class TestsDb : DbContext
    {
        public TestsDb(DbContextOptions<TestsDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>()
                .HasMany(t => t.Questions) 
                .WithOne() 
                .HasForeignKey(q => q.TestId) 
                .OnDelete(DeleteBehavior.Cascade); 
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
