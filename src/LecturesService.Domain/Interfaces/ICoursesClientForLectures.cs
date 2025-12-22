using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Domain.Interfaces
{
    public interface ICoursesClientForLectures
    {
        Task<string> GetCourseAuthorId(string courseId);
        Task<IEnumerable<string>> GetCourseBuyersIds(string courseId);
    }
}
