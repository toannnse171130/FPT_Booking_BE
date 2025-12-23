using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPT_Booking_BE.Migrations
{
    /// <inheritdoc />
    public partial class taskType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskType",
                table: "SecurityTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingID",
                keyValue: 1,
                column: "BookingDate",
                value: new DateOnly(2025, 12, 23));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingID",
                keyValue: 2,
                column: "BookingDate",
                value: new DateOnly(2025, 12, 24));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskType",
                table: "SecurityTasks");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingID",
                keyValue: 1,
                column: "BookingDate",
                value: new DateOnly(2025, 12, 19));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingID",
                keyValue: 2,
                column: "BookingDate",
                value: new DateOnly(2025, 12, 20));
        }
    }
}
