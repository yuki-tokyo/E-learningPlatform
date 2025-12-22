using Common.Exceptions;
using CoursesService.Domain.Interfaces;
using CoursesService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CoursesService.API.gRPC.Services
{
    public class CoursesGrpcServer : CoursesApi.CoursesApiBase
    {
        private readonly ICoursesService service;
        public CoursesGrpcServer(ICoursesService service)
        {
            this.service = service;
        }

        public override async Task<CourseResponse> GetCourseById(GetCourseByIdRequest request, ServerCallContext context)
        {
            try
            {
                var response = await service.GetCourseById(request.Id);

                var courseResponse = new CourseResponse
                {
                    Id = response.Id,
                    AuthorId = response.AuthorId,
                    Name = response.Name,
                    Description = response.Description,
                    Price = response.Price,
                    BuyersCount = response.BuyersCount
                };

                courseResponse.BuyersIds.AddRange(response.BuyersIds);

                return courseResponse;
            }
            catch (CourseNotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
        }
    }
}
