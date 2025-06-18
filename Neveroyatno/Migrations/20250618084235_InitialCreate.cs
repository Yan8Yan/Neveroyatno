using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neveroyatno.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamTitle",
                table: "TestResults",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExam",
                table: "TestResults",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamTitle",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "IsExam",
                table: "TestResults");
        }
    }
}
