using AutoMapper;
using Common.Exceptions;
using Common.Kafka.Messages;
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
        private readonly IKafkaProducerForCourses kafka;
        private readonly IMapper mapper;

        public CoursesService
            (ICoursesRepository repos, 
            IKafkaProducerForCourses kafka,
            IMapper mapper)
        {
            this.repos = repos;
            this.kafka = kafka;
            this.mapper = mapper;
        }
        public async Task AddCourse(string name, string description, double price, string currentUserId)
        {
            var course = await repos.AddCourse(name, description, price, currentUserId);
            var mappedCourse = mapper.Map<CourseMessage>(course);

            mappedCourse.Method = CourseMethods.Add;

            await kafka.Produce(mappedCourse);
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

            var msg = new CourseMessage { Id = id };

            msg.Method = CourseMethods.DeleteCourse;

            await kafka.Produce(msg);
        }

        public async Task<Course> GetCourseById(string id)
        {
            var course = await repos.GetCourseById(id);

            if (course == null)
            {
                throw new CourseNotFoundException("Курс не найден.");
            }

            return course;
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

            var msg = new CourseMessage { Id = id, Description = description };

            msg.Method = CourseMethods.UpdateDescription;

            await kafka.Produce(msg);
        }

        public async Task UpdateCourseName(string id, string name, string currentUserId)
        {
            var result = await repos.UpdateCourseName(id, name, currentUserId);

            if (result == 0)
            {
                throw new CourseChangesException("Курс не найден/не принадлежит вам.");
            }

            var msg = new CourseMessage { Id = id, Name = name };

            msg.Method = CourseMethods.UpdateName;

            await kafka.Produce(msg);
        }

        public async Task UpdateCoursePrice(string id, double price, string currentUserId)
        {
            var result = await repos.UpdateCoursePrice(id, price, currentUserId);

            if (result == 0)
            {
                throw new CourseChangesException("Курс не найден/не принадлежит вам.");
            }

            var msg = new CourseMessage { Id = id, Price = price };

            msg.Method = CourseMethods.UpdatePrice;

            await kafka.Produce(msg);
        }
    }
}
