using CoursesService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Infrastructure.Data
{
    public class CoursesDb : DbContext
    {
        public CoursesDb(DbContextOptions<CoursesDb> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
    }
}
