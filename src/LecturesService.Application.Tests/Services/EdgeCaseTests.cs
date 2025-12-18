using LecturesService.Domain.Interfaces;
using Moq;
using AppLecturesService = LecturesService.Application.Services.LecturesService;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.Tests.Services
{
    public class EdgeCaseTests 
    {
        private readonly Mock<ICoursesClientForLectures> _coursesClientMock;
        private readonly Mock<ILecturesRepository> _repositoryMock;
        private readonly AppLecturesService _service;

        public EdgeCaseTests()
        {
            _coursesClientMock = new Mock<ICoursesClientForLectures>();
            _repositoryMock = new Mock<ILecturesRepository>();
            _service = new AppLecturesService(_coursesClientMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddLecture_WhenContentIsTooLong_ShouldValidate()
        {
            // Arrange
            var courseId = "course-123";
            var currentUserId = "user-456";
            var authorId = "user-456";
            var name = "Лекция 1";
            var veryLongContent = new string('a', 20001);

            _coursesClientMock
                .Setup(x => x.GetCourseAuthorId(courseId))
                .ReturnsAsync(authorId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddLecture(courseId, currentUserId, name, veryLongContent));

            Assert.Contains("Слишком длинное содержание лекции", exception.Message);
        }
    }
}
