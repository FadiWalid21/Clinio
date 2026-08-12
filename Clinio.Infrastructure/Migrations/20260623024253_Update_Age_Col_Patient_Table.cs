using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Age_Col_Patient_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Patients");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f2ec49c5-b4ca-4adf-9ec7-ed1d83db5f56");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e4e0dcab-3adb-459f-8783-7351b1d46e2c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "c44471bd-4f90-48f5-8a4a-bd88ad0c9533");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "97021e8b-bbdd-4f7a-a28a-ff36c371b214");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1440ec76-8f49-4b1a-aa75-f9a500c73fb0", "AQAAAAIAAYagAAAAEEmGu4AlpAcEh2RaAT1jzqRIuczuDhGVJdfsQ0XPWnef/zO0gxfeiwSq+r+MtDNfwg==", "480ebd6b-a6b0-4cfd-8d94-dceacc487ec5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "67f11529-3bad-4aff-9fbe-3908ca956ba3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "52ef8661-57c5-46de-b95b-aae17efde7a9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "13905c1f-1a50-4f76-aca7-1edfc507fa98");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "ceb488b7-fba9-454a-a29f-f5c5050284ab");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cee5abf2-4036-4326-bb4b-ba1c8566aa5e", "AQAAAAIAAYagAAAAEA3zDP8VpD9H8+pwy9wx/xrl7GiRn5W/1/0LOzZdG+o7cj0NqJp1jOuZMmJCbqHnzg==", "cb1f4e1e-6ed2-4abd-9635-dca32a62ed3a" });
        }
    }
}
