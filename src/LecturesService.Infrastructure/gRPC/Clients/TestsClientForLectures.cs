using Common.Exceptions;
using Grpc.Core;
using LecturesService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Protos;

namespace LecturesService.Infrastructure.gRPC.Clients
{
    public class TestsClientForLectures : ITestsClientForLectures
    {
        private readonly TestsApi.TestsApiClient client;

        public TestsClientForLectures(TestsApi.TestsApiClient client)
        {
            this.client = client;
        }

        public async Task DeleteTestsByLectureId(string lectureId, string currentUserId)
        {
            try
            {
                await client.DeleteTestsByLectureIdAsync(new DeleteTestsByLectureIdRequest
                {
                    LectureId = lectureId,
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
