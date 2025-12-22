using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Domain.Interfaces.Tests
{
    public interface ITestsService
    {
        Task AddTest(string lectureId, string name, string currentUserId);
        Task ChangeTestName(string testId, string currentUserId, string newName);
        Task DeleteTest(string testId, string currentUserId);
        Task<bool> PassTheTest(string testId, List<int> answers, string currentUserId);
    }
}
