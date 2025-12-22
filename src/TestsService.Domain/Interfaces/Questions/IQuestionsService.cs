using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Domain.Interfaces.Questions
{
    public interface IQuestionsService
    {
        Task AddQuestion(string testId, List<string> answerOptions, int rightAnswer, string content, string currentUserId);
        Task ChangeQuestionContent(string questionId, string currentUserId, string newContent);
        Task DeleteQuestion(string questionId, string currentUserId);
    }
}
