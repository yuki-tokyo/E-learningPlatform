using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Application.DTO.Requests.Questions
{
    public class ChangeQuestionContentRequest
    {
        public required string Content { get; set; }
    }
}
