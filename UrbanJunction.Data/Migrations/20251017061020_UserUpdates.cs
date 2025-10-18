using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PreferredTheme",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "7abf68b8-9a9e-496e-b5e6-c1201630d10b", "AQAAAAIAAYagAAAAEE0UJZg1bw2unWAh2Dis0TZXCq2uEHii6mTxEjkNPSlXmRu0Wd9b1kG/LUJVGu5IDQ==", "4f0cbcd6-a339-471c-bfca-470e16f48e29", "Kaloqn" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3ea6571e-75aa-4256-b803-149723c25fbe", "AQAAAAIAAYagAAAAEMWhjn23Lb9sOtuXskCtzMXPeN+2VA0kNm53tnnc9g0eyfb9B0yErlU6mTT0K+JP2g==", "e1d26a78-94ba-4d67-bd91-9b51b08f0dcb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "870b6e0e-51e5-46f5-88f5-9bdf86c4e183", "AQAAAAIAAYagAAAAEKKEd2oKLAGePVwfNYi8qpAWOnMkM5vtSpuh1LV2Ku6Ut4S5MifSCV+OQkcCDGAk7g==", "a7276c35-d57a-409d-9eb3-af5646c67295" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 17, 6, 10, 20, 199, DateTimeKind.Utc).AddTicks(5246));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 17, 6, 10, 20, 199, DateTimeKind.Utc).AddTicks(5254));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 17, 6, 10, 20, 199, DateTimeKind.Utc).AddTicks(5257));
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
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5");

            migrationBuilder.AlterColumn<string>(
                name: "PreferredTheme",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PreferredTheme", "ProfilePictureUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "3ad674e3-3797-41ba-b980-9b2e85c32a51", 0, "ff95aca3-72f5-490e-a7aa-8c10d4c0aa1b", "UrbanUser", "musicfan@urban.com", true, false, null, "MUSICFAN@URBAN.COM", "MUSICFAN@URBAN.COM", "AQAAAAIAAYagAAAAEPVN+8Khka8kpJUfP3TTyhXFz9dWP918lMLcO7Uyf85+Xr9jw+Ws68qRwcEK5C/Mug==", null, false, "Light", null, "592fc11a-f01d-49b5-a936-a3d9c25a7c95", false, "Raq" },
                    { "93e5df7b-fb35-46d7-bd8c-7b88546ac77e", 0, "9c22d82b-cb5b-4543-8da6-e15e3f390ef6", "UrbanUser", "artlover@urban.com", true, false, null, "ARTLOVER@URBAN.COM", "ARTLOVER@URBAN.COM", "AQAAAAIAAYagAAAAEK+CgwU4LpXtV/yAfLm3rWsohgAlhAnzz32pmAs1vOhAm/tde1mi1IW3WSrmVkxPQA==", null, false, "Dark", null, "2e57f2b8-0801-48e5-9a45-c3fde60e01c3", false, "Emo" },
                    { "c5859895-19f2-47da-ae19-569400ee20d5", 0, "2fec3f85-0021-457a-9829-d6a9566e7f3f", "UrbanUser", "fashionguru@urban.com", true, false, null, "FASHIONGURU@URBAN.COM", "FASHIONGURU@URBAN.COM", "AQAAAAIAAYagAAAAEGKR5u4+MhwH4K/pvKZoVu8pUBZCkauwJlijj3l85O9gsz3XswiHypcCMstjorHyJg==", null, false, "Dark", null, "a88fa021-142c-4a77-b335-49c6a89cd5ef", false, "Mr.Yanev" }
                });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 10, 10, 36, 55, 174, DateTimeKind.Utc).AddTicks(3164));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 10, 10, 36, 55, 174, DateTimeKind.Utc).AddTicks(3170));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 10, 10, 36, 55, 174, DateTimeKind.Utc).AddTicks(3173));
        }
    }
}
