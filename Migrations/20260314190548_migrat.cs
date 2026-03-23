using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bus_Booking_System.Migrations
{
    /// <inheritdoc />
    public partial class migrat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "SeatReservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_ScheduleId",
                table: "SeatReservations",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatReservations_Schedules_ScheduleId",
                table: "SeatReservations",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatReservations_Schedules_ScheduleId",
                table: "SeatReservations");

            migrationBuilder.DropIndex(
                name: "IX_SeatReservations_ScheduleId",
                table: "SeatReservations");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "SeatReservations");
        }
    }
}
