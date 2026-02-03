using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PracticeLogger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.Sql(
                "INSERT INTO \"Users\" (\"Id\", \"Username\", \"PasswordHash\") VALUES (1, 'legacy', '') " +
                "ON CONFLICT (\"Id\") DO NOTHING;");
            migrationBuilder.Sql(
                "SELECT setval(pg_get_serial_sequence('\"Users\"', 'Id'), " +
                "(SELECT MAX(\"Id\") FROM \"Users\"));");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "practice_sessions",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE practice_sessions SET \"UserId\" = 1 WHERE \"UserId\" IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "practice_sessions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_practice_sessions_UserId",
                table: "practice_sessions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_practice_sessions_Users_UserId",
                table: "practice_sessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_practice_sessions_Users_UserId",
                table: "practice_sessions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_practice_sessions_UserId",
                table: "practice_sessions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "practice_sessions");
        }
    }
}
