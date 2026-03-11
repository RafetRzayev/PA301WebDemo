using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pa301Fiorelle.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIconFromSocialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Socials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Socials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
