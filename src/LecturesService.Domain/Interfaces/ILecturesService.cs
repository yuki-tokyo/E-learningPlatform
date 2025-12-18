using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Domain.Interfaces
{
    public interface ILecturesService
    {
        Task AddLecture(string courseId, string currentUserId, string name, string content);
        Task ChangeLectureName(string lectureId, string currentUserId, string newName);
        Task ChangeLectureContent(string lectureId, string currentUserId, string newContent);
        Task DeleteLecture(string lectureId, string currentUserId);
    }
}
