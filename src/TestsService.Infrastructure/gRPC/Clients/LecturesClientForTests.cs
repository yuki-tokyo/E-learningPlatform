using Common.Exceptions;
using Grpc.Core;
using LecturesService.Protos;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Domain.Interfaces.Lectures;
using TestsService.Domain.Responses;

namespace TestsService.Infrastructure.gRPC.Clients
{
    public class LecturesClientForTests : ILecturesClientForTests
    {
        private readonly LecturesApi.LecturesApiClient client;

        public LecturesClientForTests(LecturesApi.LecturesApiClient client)
        {
            this.client = client;
        }

        public async Task<LectureResponseForTests> GetLectureData(string lectureId)
        {
            try
            {
                var response = await client.GetLectureByIdAsync(new GetLectureByIdRequest { Id = lectureId });

                return new LectureResponseForTests { AuthorId = response.AuthorId, CourseId = response.CourseId };
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
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
