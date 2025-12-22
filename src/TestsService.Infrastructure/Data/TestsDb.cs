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

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
