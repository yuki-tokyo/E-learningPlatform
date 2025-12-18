using CoursesService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Interfaces
{
    public interface ICoursesRepository
    {
        Task<Course> AddCourse(string name, string description, double price, string currentUserId);
        Task<int> DeleteCourse(string id, string currentUserId);
        Task<int> UpdateCourseName(string id, string name, string currentUserId);
        Task<int> UpdateCourseDescription(string id, string description, string currentUserId);
        Task<int> UpdateCoursePrice(string id, double price, string currentUserId);
        Task<int> BuyCourse(string id, string currentUserId);
        Task<IEnumerable<Course>> GetCoursesIBought(string currentUserId);
        Task<IEnumerable<Course>> GetCoursesIPosted(string currentUserId);
        Task<Course?> GetCourseById(string id);
    }
}
