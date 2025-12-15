using CoursesService.Domain.Entities;
using SearchService.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Infrastructure.Extensions
{
    public static class GrpcExtensions
    {
        public static Course ToDomainModel(this CourseResponse grpcCourse)
        {
            if (grpcCourse == null) return null;

            return new Course
            {
                Id = grpcCourse.Id,
                Name = grpcCourse.Name,
                Description = grpcCourse.Description,
                AuthorId = grpcCourse.AuthorId,
                Price = grpcCourse.Price,
                BuyersCount = grpcCourse.BuyersCount,
                CreatedAt = grpcCourse.CreatedAt?.ToDateTime() ?? DateTime.UtcNow
            };
        }


        public static List<Course> ToDomainModels(
            this IEnumerable<CourseResponse> grpcCourses)
        {
            return grpcCourses?
                .Where(c => c != null)
                .Select(c => c.ToDomainModel())
                .ToList() ?? new List<Course>();
        }
    }
}
