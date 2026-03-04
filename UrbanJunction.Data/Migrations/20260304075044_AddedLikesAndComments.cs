using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLikesAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "SubcategoryId", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Check out the East Side Gallery and RAW Gelände!", new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Best Graffiti Spots in Berlin", "93e5df7b-fb35-46d7-bd8c-7b88546ac77e" },
                    { 2, "The scene is raw and authentic. Worth experiencing!", new DateTime(2026, 2, 26, 11, 31, 55, 924, DateTimeKind.Utc).AddTicks(6425), 2, "Underground Techno in Detroit", "93e5df7b-fb35-46d7-bd8c-7b88546ac77e" },
                    { 3, "Baggy is back. Sneakers are getting chunkier than ever.", new DateTime(2026, 2, 26, 11, 31, 55, 924, DateTimeKind.Utc).AddTicks(6429), 3, "Streetwear Trends for 2025", "93e5df7b-fb35-46d7-bd8c-7b88546ac77e" }
                });
        }
    }
}
