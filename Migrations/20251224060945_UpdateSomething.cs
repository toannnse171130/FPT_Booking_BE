using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPT_Booking_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSomething : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingID",
                keyValue: 1,
                column: "BookingDate",
                value: new DateOnly(2025, 12, 24));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingID",
                keyValue: 2,
                column: "BookingDate",
                value: new DateOnly(2025, 12, 25));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
