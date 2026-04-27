using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryoTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Samples",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patients");
        }
    }
}
