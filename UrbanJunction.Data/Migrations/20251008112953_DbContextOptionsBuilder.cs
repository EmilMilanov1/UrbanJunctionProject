using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class DbContextOptionsBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfileImageUrl", "SecurityStamp" },
                values: new object[] { "a8b595d4-a49b-418a-bdfb-2f3be63f5bfe", "AQAAAAIAAYagAAAAEJWPbuwSxJH0zqWiz7RdroN9QIWSRSBYC+drJoei3Ec9rnzy/RlgaywJeKl9fkZGVw==", null, "2d9ae951-bf64-4a58-becf-f1178ce4895a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfileImageUrl", "SecurityStamp" },
                values: new object[] { "68ac6848-84ea-47cc-8cc4-a15575276895", "AQAAAAIAAYagAAAAEN+NNYwbn+WdRmbkebarcu0CcEjb2gjG9w/xvP164HnV4TN5OpEKloMZJLKKcSxWPg==", null, "9268050d-aac3-406f-80c7-b7bea5d27ade" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfileImageUrl", "SecurityStamp" },
                values: new object[] { "c9c2f3b6-341e-4791-ad06-83f358d3c8d8", "AQAAAAIAAYagAAAAEMUNBIbGjhoLvV0SOgUZeba97+TJ5zvYDfESp9UjFgNLqd1tZeyM7d7S6I8YlMHLdQ==", null, "eb7a085d-4e1d-4882-b9e6-c96605b4d920" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 8, 11, 29, 53, 466, DateTimeKind.Utc).AddTicks(3919));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 8, 11, 29, 53, 466, DateTimeKind.Utc).AddTicks(3937));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 8, 11, 29, 53, 466, DateTimeKind.Utc).AddTicks(3940));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "062b44b0-59a7-4f0c-9217-a508c95fea78", "AQAAAAIAAYagAAAAEA/bda6evxa+9sT/r9brCEoOwhjaMDghpCExr9Bi6WRZR2vL1RI6huji0RaMcy5gWg==", "1d1e6f83-67ec-4685-9d58-c3c235630802" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cae68e4b-53d5-495d-aff6-409dbd5df5d2", "AQAAAAIAAYagAAAAEKSbIhgQLi1OeiCfYmwqYtD53Xb19wKCnNy6DMPJIWlqdjn+5lozrcfbLJ3yH7CHwg==", "f6a39e6e-cf13-4bd4-8697-0bd10716e17b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69a50113-2e2c-45d8-a198-81f306311eaf", "AQAAAAIAAYagAAAAEMKHVNHCB46ir/Kpeiaa3bT1Rxs/MzOyydOsT2KcR6+PhECChsNSu1iSbKWsn6zsFw==", "946a75a3-d8f6-44bc-85cf-afa514a93b21" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 6, 9, 1, 12, 128, DateTimeKind.Utc).AddTicks(8109));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 6, 9, 1, 12, 128, DateTimeKind.Utc).AddTicks(8116));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 10, 6, 9, 1, 12, 128, DateTimeKind.Utc).AddTicks(8120));
        }
    }
}
