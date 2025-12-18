using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.DTO.Requests
{
    public class ChangeLectureNameRequest
    {
        public required string Name { get; set; }
    }
}
