using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TestsService.Domain.Entities;
using TestsService.Domain.Interfaces.Tests;
using TestsService.Infrastructure.Data;

namespace TestsService.Infrastructure.Repos
{
    public class TestsRepository : ITestsRepository
    {
        private readonly IDbContextFactory<TestsDb> factory;

        public TestsRepository(IDbContextFactory<TestsDb> factory)
        {
            this.factory = factory;
        }

        public async Task AddCompletedTest(string testId, string completedId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var test = await db.Tests
                .Where(t => t.Id == testId)
                .Select(t => new
                {
                    t.CompletedIds 
                })
                .FirstOrDefaultAsync();

            test.CompletedIds.Add(completedId);

            await db.SaveChangesAsync();
        }

        public async Task AddTest(string lectureId, string courseId, string name, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var test = new Test { Name = name, LectureId = lectureId, AuthorId = currentUserId, CourseId = courseId };

            await db.Tests.AddAsync(test);
            await db.SaveChangesAsync();
        }

        public async Task<int> ChangeTestName(string testId, string currentUserId, string newName)
        {
            await using var db = await factory.CreateDbContextAsync();

            var updatedTests = await db.Tests
                .Where(t => t.Id == testId && t.AuthorId == currentUserId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(t => t.Name, newName));

            return updatedTests;
        }

        public async Task<int> DeleteTest(string testId, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var deletedTests = await db.Tests
                .Where(t => t.Id == testId && t.AuthorId == currentUserId)
                .ExecuteDeleteAsync();

            return deletedTests;
        }

        public async Task<Test?> GetTestById(string testId)
        {
            await using var db = await factory.CreateDbContextAsync();

            return await db.Tests
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == testId);
        }
    }
}
