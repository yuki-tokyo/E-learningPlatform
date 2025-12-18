using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.DTO.Requests
{
    public class AddLectureRequest
    {
        public required string Name { get; set; }
        public required string Content { get; set; }
        public required string CourseId { get; set; }
    }
}
