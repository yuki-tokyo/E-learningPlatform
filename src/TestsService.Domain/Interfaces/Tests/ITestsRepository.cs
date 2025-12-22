using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Domain.Entities;

namespace TestsService.Domain.Interfaces.Tests
{
    public interface ITestsRepository
    {
        Task AddTest(string lectureId, string courseId, string name, string currentUserId);
        Task<int> ChangeTestName(string testId, string currentUserId, string newName);
        Task<int> DeleteTest(string testId, string currentUserId);
        Task<Test?> GetTestById(string testId);
        Task AddCompletedTest(string testId, string completedId);
    }
}
