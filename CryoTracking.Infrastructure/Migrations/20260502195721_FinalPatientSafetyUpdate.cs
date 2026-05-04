using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryoTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalPatientSafetyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TCNo",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactInfo",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ContactInfo",
                table: "Patients",
                column: "ContactInfo",
                unique: true,
                filter: "[ContactInfo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_TCNo",
                table: "Patients",
                column: "TCNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_ContactInfo",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_TCNo",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "TCNo",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactInfo",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
