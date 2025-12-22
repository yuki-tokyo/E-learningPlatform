using AutoMapper;
using Common.Kafka.Messages.Courses;
using SearchService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.Application.AutoMapper
{
    public class SearchProfile : Profile
    {
        public SearchProfile()
        {
            CreateMap<CourseMessage, CourseForSearch>();
        }
    }
}
