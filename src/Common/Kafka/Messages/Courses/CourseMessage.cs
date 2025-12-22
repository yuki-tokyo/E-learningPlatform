using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Kafka.Messages.Courses
{
    public class CourseMessage
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public double Price { get; set; }
        public int BuyersCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public CourseMethods Method { get; set; }
    }
}
