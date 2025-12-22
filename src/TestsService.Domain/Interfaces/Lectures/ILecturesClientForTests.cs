using System;
using System.Collections.Generic;
using System.Text;
using TestsService.Domain.Responses;

namespace TestsService.Domain.Interfaces.Lectures
{
    public interface ILecturesClientForTests
    {
        Task<LectureResponseForTests> GetLectureData(string lectureId);
    }
}
