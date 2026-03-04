using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerImagePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerImagePath",
                table: "AspNetUsers");
        }
    }
}
