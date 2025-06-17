using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neveroyatno.Migrations
{
    /// <inheritdoc />
    public partial class FixLectureTestRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Tasks_Id",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tests_TestId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TaskItems");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TestId",
                table: "TaskItems",
                newName: "IX_TaskItems_TestId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Lectures",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskItems",
                table: "TaskItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TaskItems_Id",
                table: "Questions",
                column: "Id",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Tests_TestId",
                table: "TaskItems",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TaskItems_Id",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Tests_TestId",
                table: "TaskItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskItems",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tests");

            migrationBuilder.RenameTable(
                name: "TaskItems",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_TestId",
                table: "Tasks",
                newName: "IX_Tasks_TestId");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Lectures",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Tasks_Id",
                table: "Questions",
                column: "Id",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tests_TestId",
                table: "Tasks",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
