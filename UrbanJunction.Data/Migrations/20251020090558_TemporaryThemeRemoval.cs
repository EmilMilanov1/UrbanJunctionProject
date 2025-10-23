using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class TemporaryThemeRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredTheme",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a219e0fc-a8c7-48dd-b036-698401738410", "AQAAAAIAAYagAAAAEK/sH4qZzNpQkYRLFk6vAS5oELXuSsdttN6to38V2ewABC5C/3sqp3lAo1XIyKBHJw==", "87c623fc-9503-4c8c-a618-d41f5cae82f0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d42b752-c3f5-44e9-a8bc-809273e912f3", "AQAAAAIAAYagAAAAEAcQr/kxf793hAMq21jhqdxGx6SUTGujtKesoe3LDRsYfvzLiBdBYNbhyO7b8TG3pw==", "45e0a20e-617f-42b2-8636-124a2dd1cb24" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e2c5f4e-bb4e-4a64-90a1-5e896278090f", "AQAAAAIAAYagAAAAEOjNVHJj9COdgtU7ZxqFerTr87AWtrFTif4O/a2IPqMgsk8kyDEpUBWsbRuL1znzZw==", "141e3de5-893a-44b1-b55f-b2d855c91134" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredTheme",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PreferredTheme", "SecurityStamp" },
                values: new object[] { "7abf68b8-9a9e-496e-b5e6-c1201630d10b", "AQAAAAIAAYagAAAAEE0UJZg1bw2unWAh2Dis0TZXCq2uEHii6mTxEjkNPSlXmRu0Wd9b1kG/LUJVGu5IDQ==", "Light", "4f0cbcd6-a339-471c-bfca-470e16f48e29" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PreferredTheme", "SecurityStamp" },
                values: new object[] { "3ea6571e-75aa-4256-b803-149723c25fbe", "AQAAAAIAAYagAAAAEMWhjn23Lb9sOtuXskCtzMXPeN+2VA0kNm53tnnc9g0eyfb9B0yErlU6mTT0K+JP2g==", "Dark", "e1d26a78-94ba-4d67-bd91-9b51b08f0dcb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "PreferredTheme", "SecurityStamp" },
                values: new object[] { "870b6e0e-51e5-46f5-88f5-9bdf86c4e183", "AQAAAAIAAYagAAAAEKKEd2oKLAGePVwfNYi8qpAWOnMkM5vtSpuh1LV2Ku6Ut4S5MifSCV+OQkcCDGAk7g==", "Dark", "a7276c35-d57a-409d-9eb3-af5646c67295" });

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
    }
}
