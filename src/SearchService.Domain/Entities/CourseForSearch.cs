using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.Domain.Entities
{
    public class CourseForSearch
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AuthorId { get; set; }
        public double Price { get; set; }
        public int BuyersCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
