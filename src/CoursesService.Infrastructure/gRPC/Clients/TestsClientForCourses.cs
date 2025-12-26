using Common.Exceptions;
using CoursesService.Domain.Interfaces.Clients.Tests;
using Grpc.Core;
using LecturesService.Protos;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Protos;

namespace CoursesService.Infrastructure.gRPC.Clients
{
    public class TestsClientForCourses : ITestsClientForCourses
    {
        private readonly TestsApi.TestsApiClient client;

        public TestsClientForCourses(TestsApi.TestsApiClient client)
        {
            this.client = client;
        }

        public async Task DeleteTestsByCourseId(string courseId, string currentUserId)
        {
            try
            {
                await client.DeleteTestsByCourseIdAsync(new DeleteTestsByCourseIdRequest
                {
                    CourseId = courseId,
                    UserId = currentUserId
                });
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                throw new TestException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
