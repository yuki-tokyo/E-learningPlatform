using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces.Clients.Tests
{
    public interface ITestsClientForCourses
    {
        Task DeleteTestsByCourseId(string courseId, string currentUserId);
    }
}
