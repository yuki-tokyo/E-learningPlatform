using AutoMapper;
using CoursesService.Domain.Exceptions;
using CoursesService.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using AppCoursesService = CoursesService.Application.Services.CoursesService;

namespace CoursesService.Application.Tests.Services
{
    public class DeleteCourseTests
    {
        private readonly Mock<ICoursesRepository> _repos;
        private readonly Mock<IKafkaProducerForCourses> _producer;
        private readonly Mock<IMapper> _mapper;
        private readonly AppCoursesService _service;

        public DeleteCourseTests()
        {
            _repos = new Mock<ICoursesRepository>();
            _producer = new Mock<IKafkaProducerForCourses>();
            _mapper = new Mock<IMapper>();
            _service = new AppCoursesService(_repos.Object, _producer.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteCourse_WhenAuthorDoesntMatchesCurrentUser_ShouldThrowCourseChangesException()
        {
            // Arrange
            var id = "123456";
            var currentUserId = "user-456";

            _repos
                .Setup(x => x.DeleteCourse(id, currentUserId))
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
        public async Task DeleteCourse_WhenAuthorMatchesCurrentUser_ShouldDeleteCourse()
        {
            // Arrange
            var id = "123456";
            var currentUserId = "user-456";

            _repos
                .Setup(x => x.DeleteCourse(id, currentUserId))
                .ReturnsAsync(1);

            // Act 
            await _service.DeleteCourse(id, currentUserId);

            // Assert
            _repos.Verify(x =>
                x.DeleteCourse(id, currentUserId), Times.Once);
        }

    }
}
