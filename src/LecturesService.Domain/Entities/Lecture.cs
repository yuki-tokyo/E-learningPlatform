using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Domain.Entities
{
    public class Lecture
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Content { get; set; }
        public string? AuthorId { get; set; }
        public string? CourseId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
