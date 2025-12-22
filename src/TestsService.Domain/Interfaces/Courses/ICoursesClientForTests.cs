using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Domain.Interfaces.Courses
{
    public interface ICoursesClientForTests
    {
        Task<IEnumerable<string>> GetCourseBuyersIds(string courseId);
    }
}
