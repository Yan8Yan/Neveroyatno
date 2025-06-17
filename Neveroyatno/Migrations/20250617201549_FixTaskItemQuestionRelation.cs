using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neveroyatno.Migrations
{
    /// <inheritdoc />
    public partial class FixTaskItemQuestionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "TaskItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "TaskItems");
        }
    }
}
