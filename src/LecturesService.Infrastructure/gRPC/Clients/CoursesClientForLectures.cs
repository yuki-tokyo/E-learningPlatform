using Common.Exceptions;
using CoursesService.Protos;
using Grpc.Core;
using LecturesService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace LecturesService.Infrastructure.gRPC.Clients
{
    public class CoursesClientForLectures : ICoursesClientForLectures
    {
        private readonly CoursesApi.CoursesApiClient client;

        public CoursesClientForLectures(CoursesApi.CoursesApiClient client)
        {
            this.client = client;
        }

        public async Task<string> GetCourseAuthorId(string courseId)
        {
            try
            {
                var response = await client.GetCourseByIdAsync(new GetCourseByIdRequest { Id = courseId });

                return response.AuthorId;
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
