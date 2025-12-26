using AutoMapper;
using CoursesService.Domain.Exceptions;
using CoursesService.Domain.Interfaces.Clients.Lectures;
using CoursesService.Domain.Interfaces.Clients.Tests;
using CoursesService.Domain.Interfaces.Courses;
using CoursesService.Domain.Interfaces.Kafka;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using AppCoursesService = CoursesService.Application.Services.CoursesService;

namespace CoursesService.Application.Tests.Services
{
    public class UpdateCourseDescriptionTests
    {
        private readonly Mock<ICoursesRepository> _repos;
        private readonly Mock<IKafkaProducerForCourses> _producer;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILecturesClientForCourses> _lecturesClient;
        private readonly Mock<ITestsClientForCourses> _testsClient;
        private readonly AppCoursesService _service;

        public UpdateCourseDescriptionTests()
        {
            _repos = new Mock<ICoursesRepository>();
            _producer = new Mock<IKafkaProducerForCourses>();
            _mapper = new Mock<IMapper>();
            _lecturesClient = new Mock<ILecturesClientForCourses>();
            _testsClient = new Mock<ITestsClientForCourses>();
            _service = new AppCoursesService(_repos.Object, _producer.Object, _mapper.Object, _lecturesClient.Object, _testsClient.Object);
        }

        [Fact]
        public async Task UpdateCourseDescription_WhenAuthorDoesntMatchesCurrentUser_ShouldThrowCourseChangesException()
        {
            // Arrange
            var id = "123456";
            var currentUserId = "user-456";
            var desc = "cool course";

            _repos
                .Setup(x => x.UpdateCourseDescription(id, desc, currentUserId))
                .ReturnsAsync(0);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CourseChangesException>(() =>
                _service.DeleteCourse(id, currentUserId));

            Assert.Equal("Курс не найден/не принадлежит вам.", exception.Message);

            _producer.Verify(x =>
            x.Produce(It.IsAny<string>()),
            Times.Never);
        }

        [Fact]
        public async Task UpdateCourseDescription_WhenAuthorMatchesCurrentUser_ShouldUpdateCourseDesc()
        {
            // Arrange
            var id = "123456";
            var currentUserId = "user-456";
            var desc = "cool course";

            _repos
                .Setup(x => x.UpdateCourseDescription(id, desc, currentUserId))
                .ReturnsAsync(1);

            // Act 
            await _service.UpdateCourseDescription(id, desc, currentUserId);

            // Assert
            _repos.Verify(x =>
                x.UpdateCourseDescription(id, desc, currentUserId), Times.Once);
        }
    }
}
