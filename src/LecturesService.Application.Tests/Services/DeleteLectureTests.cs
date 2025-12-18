using LecturesService.Domain.Exceptions;
using LecturesService.Domain.Interfaces;
using Moq;
using AppLecturesService = LecturesService.Application.Services.LecturesService;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.Tests.Services
{
    public class DeleteLectureTests 
    {
        private readonly Mock<ICoursesClientForLectures> _coursesClientMock;
        private readonly Mock<ILecturesRepository> _repositoryMock;
        private readonly AppLecturesService _service;

        public DeleteLectureTests()
        {
            _coursesClientMock = new Mock<ICoursesClientForLectures>();
            _repositoryMock = new Mock<ILecturesRepository>();
            _service = new AppLecturesService(_coursesClientMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task DeleteLecture_WhenLectureExistsAndBelongsToUser_ShouldDelete()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";

            _repositoryMock
                .Setup(x => x.DeleteLecture(lectureId, currentUserId))
                .ReturnsAsync(1);

            // Act
            await _service.DeleteLecture(lectureId, currentUserId);

            // Assert
            _repositoryMock.Verify(x =>
                x.DeleteLecture(lectureId, currentUserId), Times.Once);
        }

        [Fact]
        public async Task DeleteLecture_WhenLectureNotFoundOrNotBelongsToUser_ShouldThrowLectureException()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";

            _repositoryMock
                .Setup(x => x.DeleteLecture(lectureId, currentUserId))
                .ReturnsAsync(0);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<LectureException>(() =>
                _service.DeleteLecture(lectureId, currentUserId));

            Assert.Equal("Лекция не найдена/не принадлежит вам.", exception.Message);
        }

        [Fact]
        public async Task DeleteLecture_WhenMultipleLecturesDeleted_ShouldWorkCorrectly()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";

            _repositoryMock
                .Setup(x => x.DeleteLecture(lectureId, currentUserId))
                .ReturnsAsync(1); // только 1 должна удалиться

            // Act
            await _service.DeleteLecture(lectureId, currentUserId);

            // Assert
            _repositoryMock.Verify(x =>
                x.DeleteLecture(lectureId, currentUserId), Times.Once);
        }
    }
}
