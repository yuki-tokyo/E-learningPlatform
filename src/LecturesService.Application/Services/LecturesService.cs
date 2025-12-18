using LecturesService.Domain.Exceptions;
using LecturesService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Application.Services
{
    public class LecturesService : ILecturesService
    {
        private readonly ICoursesClientForLectures coursesClient;
        private readonly ILecturesRepository repos;

        public LecturesService(ICoursesClientForLectures coursesClient, ILecturesRepository repos)
        {
            this.coursesClient = coursesClient;
            this.repos = repos;
        }
        public async Task AddLecture(string courseId, string currentUserId, string name, string content)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Имя/содержание лекции не могут быть пустыми");

            if (name.Length > 1000)
                throw new ArgumentException("Слишком длинное имя лекции", nameof(name));

            if (content.Length > 20000)
                throw new ArgumentException("Слишком длинное содержание лекции", nameof(content));

            var response = await coursesClient.GetCourseAuthorId(courseId);

            if (response != currentUserId)
            {
                throw new LectureException("Нельзя добавить лекцию к чужому курсу.");
            }

            await repos.AddLecture(courseId, currentUserId, name, content);
        }

        public async Task ChangeLectureContent(string lectureId, string currentUserId, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Содержание лекции не может быть пустым", nameof(newContent));

            if (newContent.Length > 20000)
                throw new ArgumentException("Слишком длинное содержание лекции", nameof(newContent));

            var response = await repos.ChangeLectureContent(lectureId, currentUserId, newContent);

            if (response == 0)
            {
                throw new LectureException("Лекция не найдена/не принадлежит вам.");
            }
        }

        public async Task ChangeLectureName(string lectureId, string currentUserId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Имя лекции не может быть пустым", nameof(newName));

            if (newName.Length > 1000)
                throw new ArgumentException("Слишком длинное имя лекции", nameof(newName));

            var response = await repos.ChangeLectureName(lectureId, currentUserId, newName);

            if (response == 0)
            {
                throw new LectureException("Лекция не найдена/не принадлежит вам.");
            }
        }

        public async Task DeleteLecture(string lectureId, string currentUserId)
        {
            var response = await repos.DeleteLecture(lectureId, currentUserId);

            if (response == 0)
            {
                throw new LectureException("Лекция не найдена/не принадлежит вам.");
            }
        }
    }
}
