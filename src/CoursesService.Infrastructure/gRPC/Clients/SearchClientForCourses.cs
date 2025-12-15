using Confluent.Kafka;
using CoursesService.Domain.Entities;
using CoursesService.Domain.Interfaces;
using CoursesService.Infrastructure.Extensions;
using Grpc.Core;
using PaymentService.Protos;
using SearchService.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Infrastructure.gRPC.Clients
{
    public class SearchClientForCourses : ISearchClientForCourses
    {
        private readonly SearchApi.SearchApiClient client;

        public SearchClientForCourses(SearchApi.SearchApiClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Course>> SearchCourses(string query)
        {
            try
            {
                var response = await client.SearchCoursesAsync(new SearchCoursesRequest { Query = query });
                var mappedResponse = response.Courses.ToDomainModels();

                return mappedResponse;
            }
            catch (RpcException ex)
            {
                throw new Exception($"Error: {ex}");
            }
        }
    }
}
