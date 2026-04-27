using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryoTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PreventDataLoss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityAssessments_Samples_SampleId",
                table: "QualityAssessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Samples_Patients_PatientId",
                table: "Samples");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityAssessments_Samples_SampleId",
                table: "QualityAssessments",
                column: "SampleId",
                principalTable: "Samples",
                principalColumn: "SampleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_Patients_PatientId",
                table: "Samples",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualityAssessments_Samples_SampleId",
                table: "QualityAssessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Samples_Patients_PatientId",
                table: "Samples");

            migrationBuilder.AddForeignKey(
                name: "FK_QualityAssessments_Samples_SampleId",
                table: "QualityAssessments",
                column: "SampleId",
                principalTable: "Samples",
                principalColumn: "SampleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_Patients_PatientId",
                table: "Samples",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
