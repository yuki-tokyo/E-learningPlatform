using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TestsService.Domain.Entities;
using TestsService.Domain.Interfaces.Questions;
using TestsService.Domain.Interfaces.Tests;

namespace TestsService.Application.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsRepository repos;
        private readonly ITestsRepository testRepos;

        public QuestionsService(IQuestionsRepository repos, ITestsRepository testRepos)
        {
            this.repos = repos;
            this.testRepos = testRepos;
        }
        public async Task AddQuestion(string testId, List<string> answerOptions, int rightAnswer, string content, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Вопрос не может быть пустым.");

            if (content.Length > 1500)
                throw new ArgumentException("Слишком длинный вопрос", nameof(content));

            if (answerOptions.Count < 2)
            {
                throw new QuestionException("Вариантов ответа должно быть не менее двух");
            }

            if (rightAnswer < 1 || rightAnswer > answerOptions.Count || answerOptions[rightAnswer-1] == null)
            {
                throw new QuestionException("Некорректно указан номер правильного ответа.");
            }

            var response = await testRepos.GetTestById(testId);

            if (response == null)
            {
                throw new QuestionException("Тест не найден");
            }
            else if (response.AuthorId != currentUserId)
            {
                throw new QuestionException("Нельзя добавить вопрос к чужому тесту");
            }

            await repos.AddQuestion(testId, answerOptions, rightAnswer, content, currentUserId);
        }

        public async Task ChangeQuestionContent(string questionId, string currentUserId, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Вопрос не может быть пустым", nameof(newContent));

            if (newContent.Length > 1500)
                throw new ArgumentException("Слишком длинный вопрос", nameof(newContent));

            var response = await repos.ChangeQuestionContent(questionId, currentUserId, newContent);

            if (response == 0)
            {
                throw new LectureException("Вопрос не найден/не принадлежит вам.");
            }
        }

        public async Task DeleteQuestion(string questionId, string currentUserId)
        {
            var response = await repos.DeleteQuestion(questionId, currentUserId);

            if (response == 0)
            {
                throw new LectureException("Вопрос не найден/не принадлежит вам.");
            }
        }
    }
}
