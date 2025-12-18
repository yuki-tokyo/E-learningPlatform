using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Domain.Interfaces
{
    public interface ILecturesRepository
    {
        Task AddLecture(string courseId, string currentUserId, string name, string content);
        Task<int> ChangeLectureName(string lectureId, string currentUserId, string newName);
        Task<int> ChangeLectureContent(string lectureId, string currentUserId, string newContent);
        Task<int> DeleteLecture(string lectureId, string currentUserId);
    }
}
