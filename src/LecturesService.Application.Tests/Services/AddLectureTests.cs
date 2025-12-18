using LecturesService.Domain.Exceptions;
using LecturesService.Domain.Interfaces;
using Moq;
using AppLecturesService = LecturesService.Application.Services.LecturesService;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.Tests.Services
{
    public class AddLectureTests 
    {
        private readonly Mock<ICoursesClientForLectures> _coursesClientMock;
        private readonly Mock<ILecturesRepository> _repositoryMock;
        private readonly AppLecturesService _service;

        public AddLectureTests()
        {
            _coursesClientMock = new Mock<ICoursesClientForLectures>();
            _repositoryMock = new Mock<ILecturesRepository>();
            _service = new AppLecturesService(_coursesClientMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddLecture_WhenAuthorMatchesCurrentUser_ShouldAddLecture()
        {

            var courseId = "course-123";
            var currentUserId = "user-456";
            var name = "Лекция 1";
            var content = "Контент лекции";

            _coursesClientMock
                .Setup(x => x.GetCourseAuthorId(courseId))
                .ReturnsAsync(currentUserId);

            _repositoryMock
                .Setup(x => x.AddLecture(courseId, currentUserId, name, content))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddLecture(courseId, currentUserId, name, content);

            // Assert
            _coursesClientMock.Verify(x => x.GetCourseAuthorId(courseId), Times.Once);
            _repositoryMock.Verify(x =>
                x.AddLecture(courseId, currentUserId, name, content), Times.Once);
        }

        [Fact]
        public async Task AddLecture_WhenAuthorDoesNotMatchCurrentUser_ShouldThrowLectureException()
        {
            // Arrange
            var courseId = "course-123";
            var currentUserId = "user-456";
            var authorId = "user-999"; 
            var name = "Лекция 1";
            var content = "Контент лекции";

            _coursesClientMock
                .Setup(x => x.GetCourseAuthorId(courseId))
                .ReturnsAsync(authorId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<LectureException>(() =>
                _service.AddLecture(courseId, currentUserId, name, content));

            Assert.Equal("Нельзя добавить лекцию к чужому курсу.", exception.Message);
            _coursesClientMock.Verify(x => x.GetCourseAuthorId(courseId), Times.Once);
            _repositoryMock.Verify(x =>
                x.AddLecture(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task AddLecture_WhenClientThrowsException_ShouldPropagateException()
        {
            // Arrange
            var courseId = "course-123";
            var currentUserId = "user-456";
            var name = "Лекция 1";
            var content = "Контент лекции";

            _coursesClientMock
                .Setup(x => x.GetCourseAuthorId(courseId))
                .ThrowsAsync(new InvalidOperationException("Сервис курсов недоступен"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddLecture(courseId, currentUserId, name, content));
        }
    }
}
