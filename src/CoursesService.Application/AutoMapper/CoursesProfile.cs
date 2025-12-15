using AutoMapper;
using Common.Kafka.Messages;
using CoursesService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Application.AutoMapper
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseMessage>();
        }
    }
}
