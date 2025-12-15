using Elastic.Clients.Elasticsearch;
using SearchService.Domain.Entities;
using SearchService.Domain.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SearchService.Application.Services
{
    public class CoursesSearchService : ICoursesSearchService
    {
        private readonly ElasticsearchClient elasticClient;
        private readonly IDatabase db;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly string _indexName;

        public CoursesSearchService(
            ElasticsearchClient elasticClient,
            IConnectionMultiplexer redis)
        {
            this.elasticClient = elasticClient;
            _indexName = "courses-index";
            db = redis.GetDatabase();
            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task IndexCourse(CourseForSearch course)
        {
            try
            {
                var response = await elasticClient.IndexAsync(course, idx => idx.Index(_indexName));

                if (!response.IsValidResponse)
                {
                    throw new Exception($"Ошибка индексации: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время индексации: {ex}");
                throw;
            }
        }

        public async Task UpdateCourseName(string courseId, string newName)
        {
            try
            {
                var response = await elasticClient.UpdateAsync<CourseForSearch, object>(
                    _indexName,
                    courseId,
                    u => u.Doc(new
                    {
                        name = newName
                    })
                );

                if (!response.IsValidResponse)
                {
                    throw new Exception($"Ошибка обновления имени: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время обновления имени: {ex}");
                throw;
            }
        }

        public async Task UpdateCourseDescription(string courseId, string newDescription)
        {
            try
            {
                var response = await elasticClient.UpdateAsync<CourseForSearch, object>(
                    _indexName,
                    courseId,
                    u => u.Doc(new
                    {
                        description = newDescription
                    })
                );

                if (!response.IsValidResponse)
                {
                    throw new Exception($"Ошибка обновления описания: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время обновления описания: {ex}");
                throw;
            }
        }

        public async Task UpdateCoursePrice(string courseId, double newPrice)
        {
            try
            {
                var response = await elasticClient.UpdateAsync<CourseForSearch, object>(
                    _indexName,
                    courseId,
                    u => u.Doc(new
                    {
                        price = newPrice
                    })
                );

                if (!response.IsValidResponse)
                {
                    throw new Exception($"Ошибка обновления ценника: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время обновления ценника: {ex}");
                throw;
            }
        }


        public async Task DeleteCourse(string courseId)
        {
            try
            {
                var response = await elasticClient.DeleteAsync<CourseForSearch>(courseId, idx => idx.Index(_indexName));

                if (!response.IsValidResponse && response.Result != Result.NotFound)
                {
                    throw new Exception($"Ошибка удаления: {response.DebugInformation}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время удаления: {ex}");
                throw;
            }
        }

        public async Task<List<CourseForSearch>> SearchCourses(string query)
        {
            string key = "courses_cache:" + query;

            var value = await db.StringGetAsync(key);

            if (!value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<CourseForSearch>>(
                    value.ToString(),
                    jsonOptions) ?? new List<CourseForSearch>();
            }

            List<CourseForSearch> courses = await SearchByCourseData(query);


            if (courses.Count > 0)
            {
                var json = JsonSerializer.Serialize(courses, jsonOptions);
                await db.StringSetAsync(key, json, TimeSpan.FromMinutes(5));
            }

            return courses;
        }

        private async Task<List<CourseForSearch>> SearchByCourseData(string query)
        {
            var response = await elasticClient.SearchAsync<CourseForSearch>(s => s
                .Indices(_indexName)
                .Query(q => q
                    .Bool(b => b
                        .Should(

                                            //   SEARCH FOR NAME

            //Prefix Search
            bs => bs.MatchPhrasePrefix(m => m.Field(f => f.Name).Query(query)),
            //Fuzzy Search
            bs => bs.Fuzzy(f => f.Field(f => f.Name).Value(query).Fuzziness(new Fuzziness("AUTO"))),
            //Wildcard
            bs => bs.Wildcard(w => w.Field(f => f.Name).Value($"*{query}*")),


                                            //   SEARCH FOR DESCRIPTION

            //Prefix Search
            bs => bs.MatchPhrasePrefix(m => m.Field(f => f.Description).Query(query)),
            //Fuzzy Search
            bs => bs.Fuzzy(f => f.Field(f => f.Description).Value(query).Fuzziness(new Fuzziness("AUTO"))),
            //Wildcard
            bs => bs.Wildcard(w => w.Field(f => f.Description).Value($"*{query}*"))
                        )
                    )
                )
                .Size(150)
            );

            if (!response.IsValidResponse || response.Documents == null)
            {
                return new List<CourseForSearch>();
            }
            else
            {
                return response.Documents.ToList();
            }

        }
    }
}
