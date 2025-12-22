using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Domain.Responses
{
    public class LectureResponseForTests
    {
        public required string AuthorId { get; set; }
        public required string CourseId { get; set; }
    }
}
