using LecturesService.Domain.Interfaces;
using Moq;
using AppLecturesService = LecturesService.Application.Services.LecturesService;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.Tests.Services
{
    public class IntegrationBehaviorTests 
    {
        private readonly Mock<ICoursesClientForLectures> _coursesClientMock;
        private readonly Mock<ILecturesRepository> _repositoryMock;
        private readonly AppLecturesService _service;

        public IntegrationBehaviorTests()
        {
            _coursesClientMock = new Mock<ICoursesClientForLectures>();
            _repositoryMock = new Mock<ILecturesRepository>();
            _service = new AppLecturesService(_coursesClientMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task ChangeLectureContent_ThenChangeLectureName_ShouldWorkInSequence()
        {
            // Arrange
            var lectureId = "lecture-123";
            var currentUserId = "user-456";
            var newContent = "Новый контент";
            var newName = "Новое имя";

            _repositoryMock
                .SetupSequence(x => x.ChangeLectureContent(lectureId, currentUserId, newContent))
                .ReturnsAsync(1)
                .ReturnsAsync(1);

            _repositoryMock
                .SetupSequence(x => x.ChangeLectureName(lectureId, currentUserId, newName))
                .ReturnsAsync(1)
                .ReturnsAsync(1);

            // Act
            await _service.ChangeLectureContent(lectureId, currentUserId, newContent);
            await _service.ChangeLectureName(lectureId, currentUserId, newName);
            await _service.ChangeLectureContent(lectureId, currentUserId, newContent);
            await _service.ChangeLectureName(lectureId, currentUserId, newName);

            // Assert
            _repositoryMock.Verify(x =>
                x.ChangeLectureContent(lectureId, currentUserId, newContent), Times.Exactly(2));
            _repositoryMock.Verify(x =>
                x.ChangeLectureName(lectureId, currentUserId, newName), Times.Exactly(2));
        }
    }
}
