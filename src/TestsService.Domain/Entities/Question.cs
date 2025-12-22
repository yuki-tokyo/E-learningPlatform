using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Domain.Entities
{
    public class Question
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Content { get; set; }
        public List<string> AnswerOptions { get; set; } = new List<string>();
        public int RightAnswer { get; set; }
        public string? AuthorId { get; set; }
        public string? TestId { get; set; }
    }
}
