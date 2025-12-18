using CoursesService.Domain.Entities;
using CoursesService.Domain.Exceptions;
using CoursesService.Domain.Interfaces;
using CoursesService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Xml.Linq;

namespace CoursesService.Infrastructure.Repos
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly IDbContextFactory<CoursesDb> factory;
        private readonly IPaymentClientForCourses paymentClient;

        public CoursesRepository(IDbContextFactory<CoursesDb> factory, IPaymentClientForCourses paymentClient)
        {
            this.factory = factory;
            this.paymentClient = paymentClient;
        }
        public async Task<Course> AddCourse(string name, string description, double price, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var course = new Course { Name = name, Description = description, Price = price, AuthorId = currentUserId };

            await db.Courses.AddAsync(course);
            await db.SaveChangesAsync();

            return course;
        }

        public async Task<int> BuyCourse(string id, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var course = await db.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return 0;
            }
            else if (course.BuyersIds.Contains(currentUserId))
            {
                return 1;
            }
            else if (course.AuthorId == currentUserId)
            {
                return 2;
            }


            course.BuyersIds.Add(currentUserId);
            course.BuyersCount++;
            await db.SaveChangesAsync();

            await paymentClient.SpendMoney(currentUserId, course.Price);

            return 3;
        }

        public async Task<int> DeleteCourse(string id, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var deletedCourses = await db.Courses
                .Where(c => c.Id == id && c.AuthorId == currentUserId)
                .ExecuteDeleteAsync();

            return deletedCourses;
        }

        public async Task<Course?> GetCourseById(string id)
        {
            await using var db = await factory.CreateDbContextAsync();

            return await db.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetCoursesIBought(string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            return await db.Courses
                .AsNoTracking()
                .Where(c => c.BuyersIds.Contains(currentUserId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesIPosted(string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            return await db.Courses
                .AsNoTracking()
                .Where(c => c.AuthorId == currentUserId)
                .ToListAsync();
        }

        public async Task<int> UpdateCourseDescription(string id, string description, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var updatedCourses = await db.Courses
                .Where(c => c.Id == id && c.AuthorId == currentUserId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(c => c.Description, description));

            return updatedCourses;
        }

        public async Task<int> UpdateCourseName(string id, string name, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var updatedCourses = await db.Courses
                .Where(c => c.Id == id && c.AuthorId == currentUserId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(c => c.Name, name));
            Console.WriteLine(updatedCourses);

            return updatedCourses;
        }

        public async Task<int> UpdateCoursePrice(string id, double price, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var updatedCourses = await db.Courses
                .Where(c => c.Id == id && c.AuthorId == currentUserId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(c => c.Price, price));

            return updatedCourses;
        }
    }
}
