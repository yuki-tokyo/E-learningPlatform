using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TestsService.Domain.Entities;
using TestsService.Domain.Interfaces.Questions;
using TestsService.Infrastructure.Data;

namespace TestsService.Infrastructure.Repos
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly IDbContextFactory<TestsDb> factory;

        public QuestionsRepository(IDbContextFactory<TestsDb> factory)
        {
            this.factory = factory;
        }

        public async Task AddQuestion
            (string testId, List<string> answerOptions, 
            int rightAnswer, string content, 
            string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var question = new Question
            {
                TestId = testId,
                Content = content,
                AnswerOptions = answerOptions,
                RightAnswer = rightAnswer,
                AuthorId = currentUserId
            };

            await db.Questions.AddAsync(question);
            await db.SaveChangesAsync();
        }

        public async Task<int> ChangeQuestionContent(string questionId, string currentUserId, string newContent)
        {
            await using var db = await factory.CreateDbContextAsync();

            var updatedQuestions = await db.Questions
                .Where(q => q.Id == questionId && q.AuthorId == currentUserId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(q => q.Content, newContent));

            return updatedQuestions;
        }

        public async Task<int> DeleteQuestionsByTestId(string testId, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var deletedQuestions = await db.Questions
                .Where(q => q.TestId == testId && q.AuthorId == currentUserId)
                .ExecuteDeleteAsync();

            return deletedQuestions;
        }

        public async Task<int> DeleteQuestion(string questionId, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var deletedQuestions = await db.Questions
                .Where(q => q.Id == questionId && q.AuthorId == currentUserId)
                .ExecuteDeleteAsync();

            return deletedQuestions;
        }

        public async Task<List<Question>> GetAllQuestionsByTestId(string testId)
        {
            await using var db = await factory.CreateDbContextAsync();

            return await db.Questions
                .AsNoTracking()
                .Where(q => q.TestId == testId)
                .ToListAsync();
        }
    }
}
