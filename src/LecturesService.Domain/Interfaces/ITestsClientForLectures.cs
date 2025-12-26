using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Domain.Interfaces
{
    public interface ITestsClientForLectures
    {
        Task DeleteTestsByLectureId(string lectureId, string currentUserId);
    }
}
