using Common.Exceptions;
using Grpc.Core;
using LecturesService.Domain.Interfaces;
using LecturesService.Protos;

namespace LecturesService.API.gRPC.Services
{
    public class LecturesGrpcServer : LecturesApi.LecturesApiBase
    {
        private readonly ILecturesService service;
        public LecturesGrpcServer(ILecturesService service)
        {
            this.service = service;
        }

        public override async Task<LectureResponse> GetLectureById(GetLectureByIdRequest request, ServerCallContext context)
        {
            try
            {
                var response = await service.GetLectureById(request.Id);

                return new LectureResponse
                {
                    Id = response.Id,
                    AuthorId = response.AuthorId,
                    Name = response.Name,
                    CourseId = response.CourseId,
                    Content = response.Content
                };
            }
            catch (LectureException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
        }
    }
}
