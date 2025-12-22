using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Domain.Entities
{
    public class Test
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? LectureId { get; set; }
        public string? AuthorId { get; set; }
        public List<string>? CompletedIds { get; set; }
        public required string CourseId { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
