using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using TestsService.Domain.Interfaces.Courses;
using TestsService.Domain.Interfaces.Lectures;
using TestsService.Domain.Interfaces.Questions;
using TestsService.Domain.Interfaces.Tests;

namespace TestsService.Application.Services
{
    public class TestsService : ITestsService
    {
        private readonly ILecturesClientForTests lecturesClient;
        private readonly ICoursesClientForTests coursesClient;
        private readonly ITestsRepository repos;
        private readonly IQuestionsRepository qrepos;

        public TestsService
            (ILecturesClientForTests lecturesClient, 
            ITestsRepository repos, 
            IQuestionsRepository qrepos, 
            ICoursesClientForTests coursesClient)
        {
            this.lecturesClient = lecturesClient;
            this.repos = repos;
            this.qrepos = qrepos;
            this.coursesClient = coursesClient;
        }

        public async Task AddTest(string lectureId, string name, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название теста не может быть пустым.");

            if (name.Length > 700)
                throw new ArgumentException("Слишком длинное название теста", nameof(name));

            var response = await lecturesClient.GetLectureData(lectureId);

            if (response.AuthorId != currentUserId)
            {
                throw new TestException("Нельзя добавить тест к чужой лекции");
            }

            await repos.AddTest(lectureId, response.CourseId, name, currentUserId);
        }

        public async Task ChangeTestName(string testId, string currentUserId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название теста не может быть пустым", nameof(newName));

            if (newName.Length > 1000)
                throw new ArgumentException("Слишком длинное название теста", nameof(newName));

            var response = await repos.ChangeTestName(testId, currentUserId, newName);

            if (response == 0)
            {
                throw new TestException("Тест не найден/не принадлежит вам.");
            }
        }

        public async Task DeleteTest(string testId, string currentUserId)
        {
            var response = await repos.DeleteTest(testId, currentUserId);

            if (response == 0)
            {
                throw new TestException("Тест не найден/не принадлежит вам.");
            }
        }

        public async Task<bool> PassTheTest(string testId, List<int> answers, string currentUserId)
        {
            int correct = 0;

            var test = await repos.GetTestById(testId);

            if (test == null)
            {
                throw new ArgumentException("Тест не найден.");
            }
            else if (test.CompletedIds.Contains(currentUserId))
            {
                throw new TestException("Вы уже прошли тест.");
            }

            var buyers = await coursesClient.GetCourseBuyersIds(test.CourseId);

            if (!buyers.Contains(currentUserId))
            {
                throw new TestException("Ошибка получения доступа к тесту.");
            }

            var questions = await qrepos.GetAllQuestionsByTestId(testId);

            if(answers.Count != questions.Count)
                throw new TestException("Некорректное кол-во ответов на тест.");

            for (int i = 0; i < questions.Count; i++)
            {
                if (i < answers.Count && answers[i] == questions[i].RightAnswer)
                {
                    correct++;
                }
            }

            if (correct == questions.Count)
            {
                await repos.AddCompletedTest(testId, currentUserId);
                return true;
            }

            return false;
        }
    }
}
