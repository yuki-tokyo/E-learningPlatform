using CoursesService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces
{
    public interface ISearchClientForCourses
    {
        Task<IEnumerable<Course>> SearchCourses(string query);
    }
}
