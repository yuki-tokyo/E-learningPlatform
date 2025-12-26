using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Domain.Entities;

namespace TestsService.Domain.Interfaces.Questions
{
    public interface IQuestionsRepository
    {
        Task AddQuestion(string testId, List<string> answerOptions, 
            int rightAnswer, string content, 
            string currentUserId);
        Task<int> ChangeQuestionContent(string questionId, string currentUserId, string newContent);
        Task<int> DeleteQuestion(string questionId, string currentUserId);
        Task<int> DeleteQuestionsByTestId(string testId, string currentUserId);
        Task<List<Question>> GetAllQuestionsByTestId(string testId);
    }
}
