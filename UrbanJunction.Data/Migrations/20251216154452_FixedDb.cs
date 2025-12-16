using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbanJunction.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7c09dae5-1eee-4c54-bbb8-098edc5c068d", "AQAAAAIAAYagAAAAEEwmBlCMZkdp0UW+rTFYVIlIwyVjUxhI5F3xwqYjlTJKHHFmxRq5TtrRZQiffbUdHA==", "3e3b25d9-56f7-4a6d-b48b-4479805cf18d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1736277-1b0d-42dc-8977-f0ead82c5c39", "AQAAAAIAAYagAAAAENM1dLs83IQCP/5Q6hu23j+StAnXb2gHgWjhrahBlKxZgFFAb50MMtM4l47pj7WkpA==", "7a3075e4-a4c9-481a-8818-d3cfcbda0046" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c5859895-19f2-47da-ae19-569400ee20d5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6bf11ee3-f2a0-4bb5-94cf-297779e03fa8", "AQAAAAIAAYagAAAAEOXcXPPp+T5c8nrQD88KEJOgNVXLhqyIx4D55YwWfEVRgc5vB4ANPNiM8cXeFRyqrA==", "e9bb6083-adf0-4541-bd8a-7a82cc1b55a7" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 16, 15, 44, 51, 697, DateTimeKind.Utc).AddTicks(9347));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 16, 15, 44, 51, 697, DateTimeKind.Utc).AddTicks(9354));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 12, 16, 15, 44, 51, 697, DateTimeKind.Utc).AddTicks(9357));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
