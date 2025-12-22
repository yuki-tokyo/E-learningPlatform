using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Application.DTO.Requests.Tests
{
    public class AddTestRequest
    {
        public required string Name { get; set; }
        public required string LectureId { get; set; }
    }
}
