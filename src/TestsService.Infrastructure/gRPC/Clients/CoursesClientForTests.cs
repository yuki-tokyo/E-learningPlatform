using Common.Exceptions;
using CoursesService.Protos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Domain.Interfaces.Courses;

namespace TestsService.Infrastructure.gRPC.Clients
{
    public class CoursesClientForTests : ICoursesClientForTests
    {
        private readonly CoursesApi.CoursesApiClient client;

        public CoursesClientForTests(CoursesApi.CoursesApiClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<string>> GetCourseBuyersIds(string courseId)
        {
            try
            {
                var response = await client.GetCourseByIdAsync(new GetCourseByIdRequest { Id = courseId });

                return response.BuyersIds;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                throw new CourseNotFoundException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }

    }
}
