using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Application.DTO.Requests.Questions
{
    public class AddQuestionRequest
    {
        public required string Content { get; set; }
        public required List<string> AnswerOptions { get; set; } 
        public required int RightAnswer { get; set; }
    }
}
