using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Application.DTO.Requests
{
    public class ChangeCourseDescRequest
    {
        public required string Description { get; set; }
    }
}
