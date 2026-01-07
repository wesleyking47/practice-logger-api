using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticeLogger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSessionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PracticeSessions",
                table: "PracticeSessions");

            migrationBuilder.RenameTable(
                name: "PracticeSessions",
                newName: "practice_sessions");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "practice_sessions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Activity",
                table: "practice_sessions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_practice_sessions",
                table: "practice_sessions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_practice_sessions",
                table: "practice_sessions");

            migrationBuilder.RenameTable(
                name: "practice_sessions",
                newName: "PracticeSessions");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "PracticeSessions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Activity",
                table: "PracticeSessions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PracticeSessions",
                table: "PracticeSessions",
                column: "Id");
        }
    }
}
