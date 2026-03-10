using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pa301Fiorelle.Migrations
{
    /// <inheritdoc />
    public partial class AddCardImagetoBio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FooterImageName",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterImageName",
                table: "Bios");
        }
    }
}
