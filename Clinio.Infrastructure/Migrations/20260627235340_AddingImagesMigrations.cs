using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingImagesMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClinicImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsCover = table.Column<bool>(type: "bit", nullable: false),
                    ClinicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicImages_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1655c707-e218-48ec-8ff6-1dea1b9da936");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1d5127a7-4a2b-49d4-b1cc-2ca94241f10b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "ae737bfd-0c28-4ee6-9ab4-887e6f813bbe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "65366f27-b2e9-4e81-aacb-dea806e2d1e4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Image", "ImageFileName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e6fac817-2207-4ead-9388-cea93e9423fa", null, null, "AQAAAAIAAYagAAAAEEN/AEMcikP3DPxYqKutyb1nFt4D8jcKLkWwWjhy2gGvSarKuZKsEN0tIc5eUOHc8w==", "7f9bf829-4f27-4a8a-8732-5b2611c7c595" });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicImages_ClinicId",
                table: "ClinicImages",
                column: "ClinicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicImages");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "AspNetUsers");

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
    }
}
