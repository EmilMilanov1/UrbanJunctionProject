using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePictureUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                newName: "ProfilePictureUrl");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff95aca3-72f5-490e-a7aa-8c10d4c0aa1b", "AQAAAAIAAYagAAAAEPVN+8Khka8kpJUfP3TTyhXFz9dWP918lMLcO7Uyf85+Xr9jw+Ws68qRwcEK5C/Mug==", "592fc11a-f01d-49b5-a936-a3d9c25a7c95" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9c22d82b-cb5b-4543-8da6-e15e3f390ef6", "AQAAAAIAAYagAAAAEK+CgwU4LpXtV/yAfLm3rWsohgAlhAnzz32pmAs1vOhAm/tde1mi1IW3WSrmVkxPQA==", "2e57f2b8-0801-48e5-9a45-c3fde60e01c3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2fec3f85-0021-457a-9829-d6a9566e7f3f", "AQAAAAIAAYagAAAAEGKR5u4+MhwH4K/pvKZoVu8pUBZCkauwJlijj3l85O9gsz3XswiHypcCMstjorHyJg==", "a88fa021-142c-4a77-b335-49c6a89cd5ef" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                newName: "ProfileImageUrl");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8b595d4-a49b-418a-bdfb-2f3be63f5bfe", "AQAAAAIAAYagAAAAEJWPbuwSxJH0zqWiz7RdroN9QIWSRSBYC+drJoei3Ec9rnzy/RlgaywJeKl9fkZGVw==", "2d9ae951-bf64-4a58-becf-f1178ce4895a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "68ac6848-84ea-47cc-8cc4-a15575276895", "AQAAAAIAAYagAAAAEN+NNYwbn+WdRmbkebarcu0CcEjb2gjG9w/xvP164HnV4TN5OpEKloMZJLKKcSxWPg==", "9268050d-aac3-406f-80c7-b7bea5d27ade" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9c2f3b6-341e-4791-ad06-83f358d3c8d8", "AQAAAAIAAYagAAAAEMUNBIbGjhoLvV0SOgUZeba97+TJ5zvYDfESp9UjFgNLqd1tZeyM7d7S6I8YlMHLdQ==", "eb7a085d-4e1d-4882-b9e6-c96605b4d920" });

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
    }
}
