using CoursesService.Domain.Entities;
using CoursesService.Domain.Exceptions;
using CoursesService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Application.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository repos;

        public CoursesService(ICoursesRepository repos)
        {
            this.repos = repos;
        }
        public async Task AddCourse(string name, string description, double price, string currentUserId)
        {
            await repos.AddCourse(name, description, price, currentUserId);
        }

        public async Task BuyCourse(string id, string currentUserId)
        {
            var result = await repos.BuyCourse(id, currentUserId);
            
            if (result == 0)
            {
                throw new CoursePurchaseException("Курс не найден.");
            }
            else if (result == 1)
            {
                throw new CoursePurchaseException("Вы уже купили данный курс.");
            }
            else if (result == 2)
            {
                throw new CoursePurchaseException("Вы не можете купить свой курс.");
            }
        }

        public async Task DeleteCourse(string id, string currentUserId)
        {
            var result = await repos.DeleteCourse(id, currentUserId);

            if (result == 0)
            {
                throw new CourseChangesException("Курс не найден/не принадлежит вам.");
            }
        }

        public async Task<IEnumerable<Course>> GetCoursesIBought(string currentUserId)
        {
            return await repos.GetCoursesIBought(currentUserId);
        }

        public async Task<IEnumerable<Course>> GetCoursesIPosted(string currentUserId)
        {
            return await repos.GetCoursesIPosted(currentUserId);
        }

        public async Task UpdateCourseDescription(string id, string description, string currentUserId)
        {
            var result = await repos.UpdateCourseDescription(id, description, currentUserId);

            if (result == 0)
            {
                throw new CourseChangesException("Курс не найден/не принадлежит вам.");
            }
        }

        public async Task UpdateCourseName(string id, string name, string currentUserId)
        {
            var result = await repos.UpdateCourseName(id, name, currentUserId);

            if (result == 0)
            {
                throw new CourseChangesException("Курс не найден/не принадлежит вам.");
            }
        }

        public async Task UpdateCoursePrice(string id, double price, string currentUserId)
        {
            var result = await repos.UpdateCoursePrice(id, price, currentUserId);

            if (result == 0)
            {
                throw new CourseChangesException("Курс не найден/не принадлежит вам.");
            }
        }
    }
}
