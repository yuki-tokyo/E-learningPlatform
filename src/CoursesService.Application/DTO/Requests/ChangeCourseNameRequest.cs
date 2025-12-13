using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Application.DTO.Requests
{
    public class ChangeCourseNameRequest
    {
        public required string Name { get; set; }
    }
}
