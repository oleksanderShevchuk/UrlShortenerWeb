using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortenerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeToShortUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ShortUrls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "ShortUrls");
        }
    }
}
