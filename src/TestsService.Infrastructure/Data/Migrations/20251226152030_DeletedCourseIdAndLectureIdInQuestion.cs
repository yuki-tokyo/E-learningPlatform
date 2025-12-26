using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeletedCourseIdAndLectureIdInQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "Questions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LectureId",
                table: "Questions",
                type: "text",
                nullable: true);
        }
    }
}
