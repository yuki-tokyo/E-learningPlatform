using Common.Exceptions;
using CoursesService.Domain.Interfaces.Clients.Lectures;
using Grpc.Core;
using LecturesService.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Infrastructure.gRPC.Clients
{
    public class LecturesClientForCourses : ILecturesClientForCourses
    {
        private readonly LecturesApi.LecturesApiClient client;

        public LecturesClientForCourses(LecturesApi.LecturesApiClient client)
        {
            this.client = client;
        }

        public async Task DeleteLecturesByCourseId(string courseId, string currentUserId)
        {
            try
            {
                await client.DeleteLecturesByCourseIdAsync(new DeleteLecturesByCourseIdRequest 
                { 
                    CourseId = courseId,
                    UserId = currentUserId
                });
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                throw new LectureException(ex.Status.Detail);
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
