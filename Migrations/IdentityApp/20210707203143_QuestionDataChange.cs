using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizAndAnswer.Migrations.IdentityApp
{
    public partial class QuestionDataChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionCount",
                table: "UserQuestionData",
                newName: "MaxPoints");

            migrationBuilder.RenameColumn(
                name: "AnsweredCorrectly",
                table: "UserQuestionData",
                newName: "CorrectPoints");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmitDate",
                table: "UserQuestionData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmitDate",
                table: "UserQuestionData");

            migrationBuilder.RenameColumn(
                name: "MaxPoints",
                table: "UserQuestionData",
                newName: "QuestionCount");

            migrationBuilder.RenameColumn(
                name: "CorrectPoints",
                table: "UserQuestionData",
                newName: "AnsweredCorrectly");
        }
    }
}
