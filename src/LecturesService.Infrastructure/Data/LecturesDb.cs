using LecturesService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Infrastructure.Data
{
    public class LecturesDb : DbContext
    {
        public LecturesDb(DbContextOptions<LecturesDb> options) : base(options) { }

        public DbSet<Lecture> Lectures { get; set; }
    }
}
