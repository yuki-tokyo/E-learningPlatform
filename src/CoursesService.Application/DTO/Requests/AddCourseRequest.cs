using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Application.DTO.Requests
{
    public class AddCourseRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double Price { get; set; }
    }
}
