using SearchService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.Domain.Interfaces
{
    public interface ICoursesSearchService
    {
        Task IndexCourse(CourseForSearch course);
        Task UpdateCourseName(string courseId, string newName);
        Task UpdateCourseDescription(string courseId, string newDescription);
        Task UpdateCoursePrice(string courseId, double newPrice);
        Task DeleteCourse(string courseId);
        Task<List<CourseForSearch>> SearchCourses(string query);
    }
}
