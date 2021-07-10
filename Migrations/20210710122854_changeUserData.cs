using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizAndAnswer.Migrations
{
    public partial class changeUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuestionData",
                table: "UserQuestionData");

            migrationBuilder.RenameTable(
                name: "UserQuestionData",
                newName: "UserData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserData",
                table: "UserData",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserData",
                table: "UserData");

            migrationBuilder.RenameTable(
                name: "UserData",
                newName: "UserQuestionData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuestionData",
                table: "UserQuestionData",
                column: "Id");
        }
    }
}
