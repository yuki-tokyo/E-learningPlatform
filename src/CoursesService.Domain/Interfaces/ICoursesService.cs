using CoursesService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces
{
    public interface ICoursesService
    {
        Task AddCourse(string name, string description, double price, string currentUserId);
        Task DeleteCourse(string id, string currentUserId);
        Task UpdateCourseName(string id, string name, string currentUserId);
        Task UpdateCourseDescription(string id, string description, string currentUserId);
        Task UpdateCoursePrice(string id, double price, string currentUserId);
        Task BuyCourse(string id, string currentUserId);
        Task<IEnumerable<Course>> GetCoursesIBought(string currentUserId);
        Task<IEnumerable<Course>> GetCoursesIPosted(string currentUserId);
    }
}
