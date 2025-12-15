using SearchService.Domain.Entities;
using SearchService.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.Infrastructure.Extensions
{
    public static class GrpcExtensions
    {
        public static CourseResponse ToGrpcResponse(this CourseForSearch course)
        {
            if (course == null) return null;

            return new CourseResponse
            {
                Id = course.Id ?? string.Empty,
                Name = course.Name ?? string.Empty,
                Description = course.Description ?? string.Empty,
                AuthorId = course.AuthorId ?? string.Empty,
                Price = course.Price,
                BuyersCount = course.BuyersCount,
                CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp
                    .FromDateTime(course.CreatedAt.ToUniversalTime())
            };
        }

        public static SearchCoursesResponse ToGrpcResponse(
            this IEnumerable<CourseForSearch> courses)
        {
            var response = new SearchCoursesResponse();
            response.Courses.AddRange(courses.Select(c => c.ToGrpcResponse()));
            return response;
        }
    }
}
