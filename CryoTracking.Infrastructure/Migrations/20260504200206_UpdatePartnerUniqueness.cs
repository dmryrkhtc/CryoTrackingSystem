using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryoTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePartnerUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartnerFullName",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerTCNo",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PartnerTCNo",
                table: "Patients",
                column: "PartnerTCNo",
                unique: true,
                filter: "[PartnerTCNo] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_PartnerTCNo",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PartnerFullName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PartnerTCNo",
                table: "Patients");
        }
    }
}
