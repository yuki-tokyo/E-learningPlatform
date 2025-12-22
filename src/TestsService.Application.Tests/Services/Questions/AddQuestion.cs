using Common.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Application.Services;
using TestsService.Domain.Entities;
using TestsService.Domain.Interfaces.Questions;
using TestsService.Domain.Interfaces.Tests;

namespace TestsService.Application.Tests.Services.Questions
{
    public class QuestionsServiceTests
    {
        private readonly Mock<IQuestionsRepository> _qRepos;
        private readonly Mock<ITestsRepository> _tRepos;
        private readonly QuestionsService _service;

        public QuestionsServiceTests()
        {
            _qRepos = new Mock<IQuestionsRepository>();
            _tRepos = new Mock<ITestsRepository>();
            _service = new QuestionsService(_qRepos.Object, _tRepos.Object);
        }


        [Fact]
        public async Task AddQuestion_WhenValidData_ShouldAddQuestion()
        {
            // Arrange
            var testId = "test-123";
            var authorId = "user-456";
            var answerOptions = new List<string> { "Да", "Нет", "Не знаю" };
            var rightAnswer = 2;
            var content = "Является ли C# строго типизированным языком?";

            var test = new Test { Id = testId, AuthorId = authorId, CourseId = "67890"};

            _tRepos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync(test);

            _qRepos
                .Setup(x => x.AddQuestion(testId, answerOptions, rightAnswer, content, authorId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddQuestion(testId, answerOptions, rightAnswer, content, authorId);

            // Assert
            _tRepos.Verify(x => x.GetTestById(testId), Times.Once);
            _qRepos.Verify(x => x.AddQuestion(
                testId, answerOptions, rightAnswer, content, authorId), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task AddQuestion_WhenContentIsEmpty_ShouldThrowArgumentException(string content)
        {
            var testId = "test-123";
            var answerOptions = new List<string> { "Да", "Нет" };
            var rightAnswer = 1;

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddQuestion(testId, answerOptions, rightAnswer, content, "user-456"));
        }

        [Fact]
        public async Task AddQuestion_WhenContentTooLong_ShouldThrowArgumentException()
        {
            var testId = "test-123";
            var answerOptions = new List<string> { "Да", "Нет" };
            var rightAnswer = 1;
            var content = new string('a', 1501);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddQuestion(testId, answerOptions, rightAnswer, content, "user-456"));

            Assert.Contains("Слишком длинный вопрос", exception.Message);
        }

        [Fact]
        public async Task AddQuestion_WhenAnswerOptionsLessThanTwo_ShouldThrowQuestionException()
        {
            var testId = "test-123";
            var answerOptions = new List<string> { "Только один" };
            var rightAnswer = 1;
            var content = "Вопрос?";

            await Assert.ThrowsAsync<QuestionException>(() =>
                _service.AddQuestion(testId, answerOptions, rightAnswer, content, "user-456"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public async Task AddQuestion_WhenRightAnswerInvalid_ShouldThrowQuestionException(int rightAnswer)
        {
            var testId = "test-123";
            var answerOptions = new List<string> { "A", "B", "C" };
            var content = "Вопрос?";

            await Assert.ThrowsAsync<QuestionException>(() =>
                _service.AddQuestion(testId, answerOptions, rightAnswer, content, "user-456"));
        }

        [Fact]
        public async Task AddQuestion_WhenAnswerOptionIsNull_ShouldThrowQuestionException()
        {
            var testId = "test-123";
            var answerOptions = new List<string> { "A", null, "C" };
            var rightAnswer = 2;
            var content = "Вопрос?";

            await Assert.ThrowsAsync<QuestionException>(() =>
                _service.AddQuestion(testId, answerOptions, rightAnswer, content, "user-456"));
        }

        [Fact]
        public async Task AddQuestion_WhenTestNotFound_ShouldThrowQuestionException()
        {
            var testId = "test-123";
            var answerOptions = new List<string> { "Да", "Нет" };
            var rightAnswer = 1;
            var content = "Вопрос?";

            _tRepos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync((Test)null);

            await Assert.ThrowsAsync<QuestionException>(() =>
                _service.AddQuestion(testId, answerOptions, rightAnswer, content, "user-456"));
        }

        [Fact]
        public async Task AddQuestion_WhenNotAuthor_ShouldThrowQuestionException()
        {
            var testId = "test-123";
            var authorId = "user-456";
            var currentUserId = "user-789"; 
            var answerOptions = new List<string> { "Да", "Нет" };
            var rightAnswer = 1;
            var content = "Вопрос?";

            var test = new Test { Id = testId, AuthorId = authorId, CourseId = "67890" }; 

            _tRepos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync(test);

            await Assert.ThrowsAsync<QuestionException>(() =>
                _service.AddQuestion(testId, answerOptions, rightAnswer, content, currentUserId));
        }


        [Fact]
        public async Task AddQuestion_WhenExactlyTwoAnswerOptions_ShouldWork()
        {
            var testId = "test-123";
            var authorId = "user-456";
            var answerOptions = new List<string> { "Да", "Нет" }; 
            var rightAnswer = 1;
            var content = "Вопрос?";

            var test = new Test { Id = testId, AuthorId = authorId, CourseId = "67890" };

            _tRepos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync(test);

            _qRepos
                .Setup(x => x.AddQuestion(testId, answerOptions, rightAnswer, content, authorId))
                .Returns(Task.CompletedTask);

            await _service.AddQuestion(testId, answerOptions, rightAnswer, content, authorId);

            _qRepos.Verify(x => x.AddQuestion(
                testId, answerOptions, rightAnswer, content, authorId), Times.Once);
        }

        [Fact]
        public async Task AddQuestion_WhenContent1500Chars_ShouldWork()
        {
            var testId = "test-123";
            var authorId = "user-456";
            var answerOptions = new List<string> { "Да", "Нет" };
            var rightAnswer = 1;
            var content = new string('a', 1500); 

            var test = new Test { Id = testId, AuthorId = authorId , CourseId = "67890" };

            _tRepos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync(test);

            _qRepos
                .Setup(x => x.AddQuestion(testId, answerOptions, rightAnswer, content, authorId))
                .Returns(Task.CompletedTask);

            await _service.AddQuestion(testId, answerOptions, rightAnswer, content, authorId);

            _qRepos.Verify(x => x.AddQuestion(
                testId, answerOptions, rightAnswer, content, authorId), Times.Once);
        }
    }
}
