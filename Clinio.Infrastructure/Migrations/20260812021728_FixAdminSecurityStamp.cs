using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminSecurityStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "46744482-5929-4078-8aa1-c55faae0a213");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "fa96bb12-85f8-4b02-9048-a54534f618f3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "35720c84-711d-45c8-aa70-3d1117d02e04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "91e63533-34bb-4dc4-8c21-919b8ae41d6d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ec6eef97-44e7-4a5c-a627-3db9ae87dd19", "AQAAAAIAAYagAAAAEBqFvRThBqB62S/TjRmrYQ0CFvcGTzxCOlKygA1SOffmPGDkqssD4eRxPmL+5mFXGw==", "fadi-admin-static-security-stamp-123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3080d6a1-a701-4bb6-b22c-35e35c362af9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "887b6db6-d196-46d4-809e-9daf1b60a22f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "cd866ab2-c8f1-43d8-bba6-cc29ffa796be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "54e36846-d4b3-4edc-893e-2a99425010c6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9cfe986f-bad0-4280-8674-381e0845f0ac", "AQAAAAIAAYagAAAAEPs9LSayszQLMsUwmhQCPgSjaByXy/aMAypbDXWlSX+w5HoQvbiSq8fw+4ZUZZ77hA==", "8da9f46b-ef09-48cc-918f-47c7578c4f28" });
        }
    }
}
