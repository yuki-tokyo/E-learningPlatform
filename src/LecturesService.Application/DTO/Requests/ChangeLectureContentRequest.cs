using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.DTO.Requests
{
    public class ChangeLectureContentRequest
    {
        public required string Content { get; set; }
    }
}
