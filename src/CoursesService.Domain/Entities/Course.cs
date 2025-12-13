using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Domain.Entities
{
    public class Course
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AuthorId { get; set; }
        public double Price { get; set; }
        public int BuyersCount { get; set; } = 0;
        public List<string> BuyersIds { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
