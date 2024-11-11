using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetBanking.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddCedulaandMontoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identification",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identification",
                schema: "Identity",
                table: "Users");
        }
    }
}
