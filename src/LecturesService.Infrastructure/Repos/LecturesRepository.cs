using LecturesService.Domain.Entities;
using LecturesService.Domain.Interfaces;
using LecturesService.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LecturesService.Infrastructure.Repos
{
    public class LecturesRepository : ILecturesRepository
    {
        private readonly IDbContextFactory<LecturesDb> factory;

        public LecturesRepository(IDbContextFactory<LecturesDb> factory)
        {
            this.factory = factory;
        }

        public async Task AddLecture(string courseId, string currentUserId, string name, string content)
        {
            await using var db = await factory.CreateDbContextAsync();

            var lecture = new Lecture { Name = name, Content = content, CourseId = courseId, AuthorId = currentUserId };

            await db.Lectures.AddAsync(lecture);
            await db.SaveChangesAsync();
        }

        public async Task<int> ChangeLectureContent(string lectureId, string currentUserId, string newContent)
        {
            await using var db = await factory.CreateDbContextAsync();

            var updatedLectures = await db.Lectures
                .Where(c => c.Id == lectureId && c.AuthorId == currentUserId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(c => c.Content, newContent));

            return updatedLectures;
        }

        public async Task<int> ChangeLectureName(string lectureId, string currentUserId, string newName)
        {
            await using var db = await factory.CreateDbContextAsync();

            var updatedLectures = await db.Lectures
                .Where(c => c.Id == lectureId && c.AuthorId == currentUserId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(c => c.Name, newName));

            return updatedLectures;
        }

        public async Task<int> DeleteLecture(string lectureId, string currentUserId)
        {
            await using var db = await factory.CreateDbContextAsync();

            var deletedLectures = await db.Lectures
                .Where(c => c.Id == lectureId && c.AuthorId == currentUserId)
                .ExecuteDeleteAsync();

            return deletedLectures;
        }
    }
}
