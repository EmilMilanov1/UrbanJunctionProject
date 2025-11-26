using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "ProfilePicturePath", "ProfilePictureUrl", "SecurityStamp", "UserName" },
                values: new object[] { "6df99356-f7df-48c3-9df4-e424ff8326c9", "VALIO", "AQAAAAIAAYagAAAAEPslZtmJfYO4ziShBUnsr03F7nwwjX/MuwCnLfUEI7VH75yr7DWTjs/9mkLQXwfq3g==", "/images/profile/default-profile.png", "/images/profile/default-profile.png", "5f56512f-6b8a-45df-b7ae-324ab2bb51d0", "Valio" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "ProfilePicturePath", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "443ef2e1-78cc-49e6-92f8-ba8652899528", "EMO", "AQAAAAIAAYagAAAAEBY408Sij5G8kXqk8k0/CUmhHPQdfbX+8ifNbunsj36WzeYR7TGTE5lRy2EsYZ120g==", "/images/profile/default-profile.png", "/images/profile/default-profile.png", "211f4826-b6dd-45be-8f58-410ada1f666b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "ProfilePicturePath", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "89831163-d802-4410-929f-4efc172d2f77", "MR.YANEV", "AQAAAAIAAYagAAAAEOHlZekeekB5Us8VIhm3r0AEB0tMrfoO1bMO/Zq52vMD5O6ofOMlY2iv4cUJ/x05Mw==", "/images/profile/default-profile.png", "/images/profile/default-profile.png", "2560f402-a32b-4529-86e4-5528aed56b7f" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 11, 14, 10, 53, 54, 719, DateTimeKind.Utc).AddTicks(3065));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 11, 14, 10, 53, 54, 719, DateTimeKind.Utc).AddTicks(3072));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 11, 14, 10, 53, 54, 719, DateTimeKind.Utc).AddTicks(3075));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "ProfilePictureUrl", "SecurityStamp", "UserName" },
                values: new object[] { "a219e0fc-a8c7-48dd-b036-698401738410", "MUSICFAN@URBAN.COM", "AQAAAAIAAYagAAAAEK/sH4qZzNpQkYRLFk6vAS5oELXuSsdttN6to38V2ewABC5C/3sqp3lAo1XIyKBHJw==", null, "87c623fc-9503-4c8c-a618-d41f5cae82f0", "Kaloqn" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "6d42b752-c3f5-44e9-a8bc-809273e912f3", "ARTLOVER@URBAN.COM", "AQAAAAIAAYagAAAAEAcQr/kxf793hAMq21jhqdxGx6SUTGujtKesoe3LDRsYfvzLiBdBYNbhyO7b8TG3pw==", null, "45e0a20e-617f-42b2-8636-124a2dd1cb24" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "1e2c5f4e-bb4e-4a64-90a1-5e896278090f", "FASHIONGURU@URBAN.COM", "AQAAAAIAAYagAAAAEOjNVHJj9COdgtU7ZxqFerTr87AWtrFTif4O/a2IPqMgsk8kyDEpUBWsbRuL1znzZw==", null, "141e3de5-893a-44b1-b55f-b2d855c91134" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 20, 9, 5, 58, 59, DateTimeKind.Utc).AddTicks(7277));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 20, 9, 5, 58, 59, DateTimeKind.Utc).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 20, 9, 5, 58, 59, DateTimeKind.Utc).AddTicks(7289));
        }
    }
}
