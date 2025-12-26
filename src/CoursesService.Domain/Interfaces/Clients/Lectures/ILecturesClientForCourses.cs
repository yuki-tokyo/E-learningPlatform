using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces.Clients.Lectures
{
    public interface ILecturesClientForCourses
    {
        Task DeleteLecturesByCourseId(string courseId, string currentUserId);
    }
}
