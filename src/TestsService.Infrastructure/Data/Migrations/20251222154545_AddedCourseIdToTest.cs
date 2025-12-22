using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestsService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCourseIdToTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "CompletedIds",
                table: "Tests",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Tests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedIds",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Tests");
        }
    }
}
