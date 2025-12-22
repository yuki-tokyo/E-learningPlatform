using LecturesService.Domain.Interfaces;
using Moq;
using AppLecturesService = LecturesService.Application.Services.LecturesService;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Exceptions;

namespace LecturesService.Application.Tests.Services
{
    public class ChangeLectureNameTests 
    {
        private readonly Mock<ICoursesClientForLectures> _coursesClientMock;
        private readonly Mock<ILecturesRepository> _repositoryMock;
        private readonly AppLecturesService _service;

        public ChangeLectureNameTests()
        {
            _coursesClientMock = new Mock<ICoursesClientForLectures>();
            _repositoryMock = new Mock<ILecturesRepository>();
            _service = new AppLecturesService(_coursesClientMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task ChangeLectureName_WhenLectureExistsAndBelongsToUser_ShouldChangeName()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var newName = "Новое имя";

            _repositoryMock
                .Setup(x => x.ChangeLectureName(lectureId, currentUserId, newName))
                .ReturnsAsync(1);

            // Act
            await _service.ChangeLectureName(lectureId, currentUserId, newName);

            // Assert
            _repositoryMock.Verify(x =>
                x.ChangeLectureName(lectureId, currentUserId, newName), Times.Once);
        }

        [Fact]
        public async Task ChangeLectureName_WhenLectureNotFoundOrNotBelongsToUser_ShouldThrowLectureException()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var newName = "Новое имя";

            _repositoryMock
                .Setup(x => x.ChangeLectureName(lectureId, currentUserId, newName))
                .ReturnsAsync(0);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<LectureException>(() =>
                _service.ChangeLectureName(lectureId, currentUserId, newName));

            Assert.Equal("Лекция не найдена/не принадлежит вам.", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ChangeLectureName_WhenNewNameIsNullOrEmpty_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.ChangeLectureName(lectureId, currentUserId, invalidName));

            _repositoryMock.Verify(x =>
                x.ChangeLectureName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }
    }
}
