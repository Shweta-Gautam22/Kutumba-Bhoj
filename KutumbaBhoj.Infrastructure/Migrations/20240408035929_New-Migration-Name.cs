using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutumbaBhoj.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Dishes");

            migrationBuilder.RenameColumn(
                name: "VerificationAt",
                table: "Users",
                newName: "VerifiedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerifiedAt",
                table: "Users",
                newName: "VerificationAt");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Dishes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
