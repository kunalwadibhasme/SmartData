using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceBackend.Migrations
{
    /// <inheritdoc />
    public partial class tenth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Appointment",
                newName: "AppointmentTime");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointment",
                newName: "AppointmentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentTime",
                table: "Appointment",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointment",
                newName: "Date");
        }
    }
}
