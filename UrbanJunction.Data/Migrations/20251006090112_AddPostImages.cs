using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImages_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "062b44b0-59a7-4f0c-9217-a508c95fea78", "AQAAAAIAAYagAAAAEA/bda6evxa+9sT/r9brCEoOwhjaMDghpCExr9Bi6WRZR2vL1RI6huji0RaMcy5gWg==", "1d1e6f83-67ec-4685-9d58-c3c235630802", "Raq" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "cae68e4b-53d5-495d-aff6-409dbd5df5d2", "AQAAAAIAAYagAAAAEKSbIhgQLi1OeiCfYmwqYtD53Xb19wKCnNy6DMPJIWlqdjn+5lozrcfbLJ3yH7CHwg==", "f6a39e6e-cf13-4bd4-8697-0bd10716e17b", "Emo" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "69a50113-2e2c-45d8-a198-81f306311eaf", "AQAAAAIAAYagAAAAEMKHVNHCB46ir/Kpeiaa3bT1Rxs/MzOyydOsT2KcR6+PhECChsNSu1iSbKWsn6zsFw==", "946a75a3-d8f6-44bc-85cf-afa514a93b21", "Mr.Yanev" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "ImagePath" },
                values: new object[] { new DateTime(2025, 10, 6, 9, 1, 12, 128, DateTimeKind.Utc).AddTicks(8109), null });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "ImagePath" },
                values: new object[] { new DateTime(2025, 10, 6, 9, 1, 12, 128, DateTimeKind.Utc).AddTicks(8116), null });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "ImagePath" },
                values: new object[] { new DateTime(2025, 10, 6, 9, 1, 12, 128, DateTimeKind.Utc).AddTicks(8120), null });

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_PostId",
                table: "PostImages",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostImages");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Posts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "351fd1f3-3f2f-43ba-a8fc-08354699d491", "AQAAAAIAAYagAAAAEEXZPW98aza25gv12NYe2CMK22wbhP0TVwgaXS1oeEkHMvKnaYlRphS7S/b9Gfbwqw==", "76725e88-2a01-40c6-b29d-15ce020d6654", "musicfan@urban.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "56f2ebe2-6df0-4966-bde7-620b7c2d69c5", "AQAAAAIAAYagAAAAEETFS+nOHXERJWeNjtn+0unHDjbl7QEXcFbzkM63wsb1y7z7PlywnP811z7CQ8Wzmw==", "520e3ec7-cafe-4280-873c-76905d47b7cc", "artlover@urban.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "1f998209-b5d9-41ab-b4e2-87cebf35fe6f", "AQAAAAIAAYagAAAAEAIJ72Q4FwGTgGzY+xY7aJOhLujoKV0art8ViXtJ0mT2nE46g+BoCCpr7kTjW3p26g==", "f0d9f9cb-2a5f-4688-9adf-5748614c657a", "fashionguru@urban.com" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 6, 12, 9, 3, 31, 76, DateTimeKind.Utc).AddTicks(1615));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 6, 12, 9, 3, 31, 76, DateTimeKind.Utc).AddTicks(1623));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 6, 12, 9, 3, 31, 76, DateTimeKind.Utc).AddTicks(1625));
        }
    }
}
