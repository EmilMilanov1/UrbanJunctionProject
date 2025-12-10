using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueEmailIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac27b58c-70aa-4c68-9c28-5c4b6192d4cb", "AQAAAAIAAYagAAAAEGz3ZdVgm7fgEMGGpEi0kQjkuLKvPS8cXRAJGzaifUgYu68IE9j1tWFTLVZVSUrfwQ==", "a0b5a4ae-a282-4d2c-935c-1d118e262220" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67676e62-96ad-466c-b922-5207abdf8eed", "AQAAAAIAAYagAAAAEKrr6Eb+wgj7/ksMpXlbdg2NF7r3xe2XdzY7uwmk70ufsAzckgEgB3GFx3Wcx/6HRw==", "b482a16b-d114-4521-a3cd-e090f9b29831" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3dff3023-68ee-4e90-908a-d0746cdf5b09", "AQAAAAIAAYagAAAAEGlOYfzbGTOEZ7nknNOA/fVjVrzbyqVj4E21NDVP2WiYxirF0n8x/+N8qm8mBda+1g==", "cf6fa4fa-d4df-4112-9eca-00782da63757" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 4, 7, 48, 16, 391, DateTimeKind.Utc).AddTicks(1824));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 4, 7, 48, 16, 391, DateTimeKind.Utc).AddTicks(1831));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 4, 7, 48, 16, 391, DateTimeKind.Utc).AddTicks(1835));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "6df99356-f7df-48c3-9df4-e424ff8326c9", "AQAAAAIAAYagAAAAEPslZtmJfYO4ziShBUnsr03F7nwwjX/MuwCnLfUEI7VH75yr7DWTjs/9mkLQXwfq3g==", "/images/profile/default-profile.png", "5f56512f-6b8a-45df-b7ae-324ab2bb51d0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "443ef2e1-78cc-49e6-92f8-ba8652899528", "AQAAAAIAAYagAAAAEBY408Sij5G8kXqk8k0/CUmhHPQdfbX+8ifNbunsj36WzeYR7TGTE5lRy2EsYZ120g==", "/images/profile/default-profile.png", "211f4826-b6dd-45be-8f58-410ada1f666b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "89831163-d802-4410-929f-4efc172d2f77", "AQAAAAIAAYagAAAAEOHlZekeekB5Us8VIhm3r0AEB0tMrfoO1bMO/Zq52vMD5O6ofOMlY2iv4cUJ/x05Mw==", "/images/profile/default-profile.png", "2560f402-a32b-4529-86e4-5528aed56b7f" });

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
    }
}
