using Common.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LecturesService.Protos;
using TestsService.Domain.Interfaces.Tests;
using TestsService.Protos;

namespace TestsService.API.gRPC.Services
{
    public class TestsGrpcServer : TestsApi.TestsApiBase
    {
        private readonly ITestsService service;
        public TestsGrpcServer(ITestsService service)
        {
            this.service = service;
        }

        public override async Task<Empty> DeleteTestsByCourseId(DeleteTestsByCourseIdRequest request, ServerCallContext context)
        {
            try
            {
                await service.DeleteTestsByCourseId(request.CourseId, request.UserId);

                return new Empty();
            }
            catch (TestException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }

        public override async Task<Empty> DeleteTestsByLectureId(DeleteTestsByLectureIdRequest request, ServerCallContext context)
        {
            try
            {
                await service.DeleteTestsByLectureId(request.LectureId, request.UserId);

                return new Empty();
            }
            catch (TestException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
        }
    }
}
