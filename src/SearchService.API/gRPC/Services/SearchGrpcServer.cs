using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SearchService.Domain.Interfaces;
using SearchService.Infrastructure.Extensions;
using SearchService.Protos;

namespace SearchService.API.gRPC.Services
{
    public class SearchGrpcServer : SearchApi.SearchApiBase
    {
        private readonly ICoursesSearchService service;
        public SearchGrpcServer(ICoursesSearchService service)
        {
            this.service = service;
        }

        public override async Task<SearchCoursesResponse> SearchCourses(SearchCoursesRequest request, ServerCallContext context)
        {
            var courses = await service.SearchCourses(request.Query);

            var grpcCourses = courses.ToGrpcResponse();

            return grpcCourses;
        }
    }
}
