using Common.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Domain.Interfaces.Courses;
using TestsService.Domain.Interfaces.Kafka;
using TestsService.Domain.Interfaces.Lectures;
using TestsService.Domain.Interfaces.Questions;
using TestsService.Domain.Interfaces.Tests;
using TestsService.Domain.Responses;
using AppTestsService = TestsService.Application.Services.TestsService;

namespace TestsService.Application.Tests.Services.Tests
{
    public class AddTestTests
    {
        private readonly Mock<ILecturesClientForTests> _lecturesClient;
        private readonly Mock<ICoursesClientForTests> _coursesClient;
        private readonly Mock<ITestsRepository> _repos;
        private readonly Mock<IQuestionsRepository> _qrepos;
        private readonly Mock<IKafkaProducerForTests> _kafka;
        private readonly AppTestsService _service;

        public AddTestTests()
        {
            _repos = new Mock<ITestsRepository>();
            _lecturesClient = new Mock<ILecturesClientForTests>();
            _coursesClient = new Mock<ICoursesClientForTests>();
            _qrepos = new Mock<IQuestionsRepository>();
            _kafka = new Mock<IKafkaProducerForTests>();
            _service = new AppTestsService
                (_lecturesClient.Object, _repos.Object, _qrepos.Object, _coursesClient.Object, _kafka.Object);
        }


        [Fact]
        public async Task AddTest_WhenAuthorMatchesCurrentUser_ShouldAddTest()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var name = "Тест-1";
            var response = new LectureResponseForTests { AuthorId = "user-456", CourseId = "course-123" };

            _lecturesClient
                .Setup(x => x.GetLectureData(lectureId))
                .ReturnsAsync(response);

            _repos
                .Setup(x => x.AddTest(lectureId, "course-123", name, currentUserId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddTest(lectureId, name, currentUserId);

            // Assert
            _lecturesClient.Verify(x => x.GetLectureData(lectureId), Times.Once);
            _repos.Verify(x =>
                x.AddTest(lectureId, "course-123", name, currentUserId), Times.Once);
        }

        [Fact]
        public async Task AddTest_WhenAuthorDoesntMatchesCurrentUser_ShouldThrowTestException()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var name = "Тест-1";
            var response = new LectureResponseForTests { AuthorId = "user-457", CourseId = "course-123" };

            _lecturesClient
                .Setup(x => x.GetLectureData(lectureId))
                .ReturnsAsync(response);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TestException>(() =>
                _service.AddTest(lectureId, name, currentUserId));

            _lecturesClient.Verify(x => x.GetLectureData(lectureId), Times.Once);
            _repos.Verify(x =>
                x.AddTest
                (It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), 
                Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task AddTest_WhenNameIsEmpty_ShouldThrowArgumentException(string name)
        {
            // Arrange
            var lectureId = "123456";
            var courseId = "38383993";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddTest(lectureId, name, courseId));

            _repos.Verify(x =>
                x.AddTest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}
