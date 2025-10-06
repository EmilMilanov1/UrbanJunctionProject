using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PreferredTheme", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "3ad674e3-3797-41ba-b980-9b2e85c32a51", 0, "351fd1f3-3f2f-43ba-a8fc-08354699d491", "UrbanUser", "musicfan@urban.com", true, false, null, "MUSICFAN@URBAN.COM", "MUSICFAN@URBAN.COM", "AQAAAAIAAYagAAAAEEXZPW98aza25gv12NYe2CMK22wbhP0TVwgaXS1oeEkHMvKnaYlRphS7S/b9Gfbwqw==", null, false, "Light", "76725e88-2a01-40c6-b29d-15ce020d6654", false, "musicfan@urban.com" },
                    { "93e5df7b-fb35-46d7-bd8c-7b88546ac77e", 0, "56f2ebe2-6df0-4966-bde7-620b7c2d69c5", "UrbanUser", "artlover@urban.com", true, false, null, "ARTLOVER@URBAN.COM", "ARTLOVER@URBAN.COM", "AQAAAAIAAYagAAAAEETFS+nOHXERJWeNjtn+0unHDjbl7QEXcFbzkM63wsb1y7z7PlywnP811z7CQ8Wzmw==", null, false, "Dark", "520e3ec7-cafe-4280-873c-76905d47b7cc", false, "artlover@urban.com" },
                    { "c5859895-19f2-47da-ae19-569400ee20d5", 0, "1f998209-b5d9-41ab-b4e2-87cebf35fe6f", "UrbanUser", "fashionguru@urban.com", true, false, null, "FASHIONGURU@URBAN.COM", "FASHIONGURU@URBAN.COM", "AQAAAAIAAYagAAAAEAIJ72Q4FwGTgGzY+xY7aJOhLujoKV0art8ViXtJ0mT2nE46g+BoCCpr7kTjW3p26g==", null, false, "Dark", "f0d9f9cb-2a5f-4688-9adf-5748614c657a", false, "fashionguru@urban.com" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Everything about art", "/img/art.jpg", "Art" },
                    { 2, "All genres of music", "/img/music.jpg", "Music" },
                    { 3, "Trends, streetwear, and design", "/img/fashion.jpg", "Fashion" }
                });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "Id", "Name", "TopicId" },
                values: new object[,]
                {
                    { 1, "Graffiti", 1 },
                    { 2, "Techno", 2 },
                    { 3, "Streetwear", 3 }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedOn", "SubcategoryId", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Check out the East Side Gallery and RAW Gelände!", new DateTime(2025, 6, 12, 9, 3, 31, 76, DateTimeKind.Utc).AddTicks(1615), 1, "Best Graffiti Spots in Berlin", "93e5df7b-fb35-46d7-bd8c-7b88546ac77e" },
                    { 2, "The scene is raw and authentic. Worth experiencing!", new DateTime(2025, 6, 12, 9, 3, 31, 76, DateTimeKind.Utc).AddTicks(1623), 2, "Underground Techno in Detroit", "93e5df7b-fb35-46d7-bd8c-7b88546ac77e" },
                    { 3, "Baggy is back. Sneakers are getting chunkier than ever.", new DateTime(2025, 6, 12, 9, 3, 31, 76, DateTimeKind.Utc).AddTicks(1625), 3, "Streetwear Trends for 2025", "93e5df7b-fb35-46d7-bd8c-7b88546ac77e" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5");

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

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e");

            migrationBuilder.DeleteData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subcategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
