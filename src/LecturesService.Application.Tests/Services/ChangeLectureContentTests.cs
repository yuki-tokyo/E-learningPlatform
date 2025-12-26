using LecturesService.Domain.Interfaces;
using Moq;
using AppLecturesService = LecturesService.Application.Services.LecturesService;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Exceptions;

namespace LecturesService.Application.Tests.Services
{
    public class ChangeLectureContentTests 
    {
        private readonly Mock<ICoursesClientForLectures> _coursesClientMock;
        private readonly Mock<ILecturesRepository> _repositoryMock;
        private readonly Mock<ITestsClientForLectures> _testsClient;
        private readonly AppLecturesService _service;

        public ChangeLectureContentTests()
        {
            _coursesClientMock = new Mock<ICoursesClientForLectures>();
            _repositoryMock = new Mock<ILecturesRepository>();
            _testsClient = new Mock<ITestsClientForLectures>();
            _service = new AppLecturesService(_coursesClientMock.Object, _repositoryMock.Object, _testsClient.Object);
        }

        [Fact]
        public async Task ChangeLectureContent_WhenLectureExistsAndBelongsToUser_ShouldChangeContent()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var newContent = "Новый контент";

            _repositoryMock
                .Setup(x => x.ChangeLectureContent(lectureId, currentUserId, newContent))
                .ReturnsAsync(1); 

            // Act
            await _service.ChangeLectureContent(lectureId, currentUserId, newContent);

            // Assert
            _repositoryMock.Verify(x =>
                x.ChangeLectureContent(lectureId, currentUserId, newContent), Times.Once);
        }

        [Fact]
        public async Task ChangeLectureContent_WhenLectureNotFoundOrNotBelongsToUser_ShouldThrowLectureException()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var newContent = "Новый контент";

            _repositoryMock
                .Setup(x => x.ChangeLectureContent(lectureId, currentUserId, newContent))
                .ReturnsAsync(0); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<LectureException>(() =>
                _service.ChangeLectureContent(lectureId, currentUserId, newContent));

            Assert.Equal("Лекция не найдена/не принадлежит вам.", exception.Message);
            _repositoryMock.Verify(x =>
                x.ChangeLectureContent(lectureId, currentUserId, newContent), Times.Once);
        }

        [Fact]
        public async Task ChangeLectureContent_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var newContent = "Новый контент";

            _repositoryMock
                .Setup(x => x.ChangeLectureContent(lectureId, currentUserId, newContent))
                .ThrowsAsync(new InvalidOperationException("Ошибка БД"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ChangeLectureContent(lectureId, currentUserId, newContent));
        }
    }
}
