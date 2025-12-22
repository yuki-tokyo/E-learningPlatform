using Common.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TestsService.Domain.Entities;
using TestsService.Domain.Interfaces.Courses;
using TestsService.Domain.Interfaces.Lectures;
using TestsService.Domain.Interfaces.Questions;
using TestsService.Domain.Interfaces.Tests;
using TestsService.Domain.Responses;
using AppTestsService = TestsService.Application.Services.TestsService;

namespace TestsService.Application.Tests.Services.Tests
{
    public class PassTheTestTests
    {
        private readonly Mock<ILecturesClientForTests> _lecturesClient;
        private readonly Mock<ICoursesClientForTests> _coursesClient;
        private readonly Mock<ITestsRepository> _repos;
        private readonly Mock<IQuestionsRepository> _qrepos;
        private readonly AppTestsService _service;

        public PassTheTestTests()
        {
            _repos = new Mock<ITestsRepository>();
            _lecturesClient = new Mock<ILecturesClientForTests>();
            _coursesClient = new Mock<ICoursesClientForTests>();
            _qrepos = new Mock<IQuestionsRepository>();
            _service = new AppTestsService
                (_lecturesClient.Object, _repos.Object, _qrepos.Object, _coursesClient.Object);
        }

        [Fact]
        public async Task PassTheTest_WhenAuthorAlreadyCompleted_ShouldTestException()
        {
            // Arrange
            List<int> answers = new List<int>{ 1, 2, 3 };
            List<string> completedIds = new List<string> { "user-2039847", "not-user-67", "user-456" };
            var test = new Test { CourseId = "67890", CompletedIds = completedIds };
            var testId = "test-123";
            var currentUserId = "user-456";

            _repos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync(test);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TestException>(() =>
                _service.PassTheTest(testId, answers, currentUserId));

            _coursesClient.Verify(x => x.GetCourseBuyersIds(It.IsAny<string>()), Times.Never);
            _qrepos.Verify(x => x.GetAllQuestionsByTestId(It.IsAny<string>()), Times.Never);
            _repos.Verify(x => x.AddCompletedTest(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PassTheTest_WhenAuthorDoesntBoughtCourse_ShouldTestException()
        {
            // Arrange
            List<int> answers = new List<int> { 1, 2, 3 };
            List<string> completedIds = new List<string> { "user-2039847", "not-user-67", "user-457" };
            List<string> buyersIds = new List<string> { "user-2039847", "not-user-67", "user-457" };
            var test = new Test { CourseId = "67890", CompletedIds = completedIds };
            var testId = "test-123";
            var currentUserId = "user-456";

            _repos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync(test);

            _coursesClient
                .Setup(x => x.GetCourseBuyersIds("67890"))
                .ReturnsAsync(buyersIds);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TestException>(() =>
                _service.PassTheTest(testId, answers, currentUserId));

            _qrepos.Verify(x => x.GetAllQuestionsByTestId(It.IsAny<string>()), Times.Never);
            _repos.Verify(x => x.AddCompletedTest(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PassTheTest_WhenUserMakeEverythingRight_ShouldPassTheTest()
        {
            // Arrange
            List<int> answers = new List<int> { 3, 4, 5 };
            List<string> completedIds = new List<string> { "user-2039847", "not-user-67", "user-457" };
            List<string> buyersIds = new List<string> { "user-2039847", "not-user-67", "user-456" };
            var test = new Test { CourseId = "67890", CompletedIds = completedIds };
            var testId = "test-123";
            var currentUserId = "user-456";


            List<Question> questions = new List<Question>{new Question { RightAnswer = 3 }, new Question { RightAnswer = 4 },
            new Question { RightAnswer = 5 }};

            _repos
                .Setup(x => x.GetTestById(testId))
                .ReturnsAsync(test);

            _coursesClient
                .Setup(x => x.GetCourseBuyersIds("67890"))
                .ReturnsAsync(buyersIds);

            _qrepos
                .Setup(x => x.GetAllQuestionsByTestId(testId))
                .ReturnsAsync(questions);

            _repos
                .Setup(x => x.AddCompletedTest(testId, currentUserId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.PassTheTest(testId, answers, currentUserId);

            // Assert
            _repos.Verify(x => x.GetTestById(testId), Times.Once);
            _coursesClient.Verify(x => x.GetCourseBuyersIds("67890"), Times.Once);
            _qrepos.Verify(x => x.GetAllQuestionsByTestId(testId), Times.Once);
            _repos.Verify(x => x.AddCompletedTest(testId, currentUserId), Times.Once);
        }
    }
}
