using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMessageReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedOn",
                table: "ContactMessages",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "RepliedOn",
                table: "ContactMessages");
        }
    }
}
