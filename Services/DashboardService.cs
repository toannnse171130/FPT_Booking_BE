using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly FptFacilityBookingContext _context;

        public DashboardService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsResponse> GetStatsAsync()
        {
            var response = new DashboardStatsResponse();

            response.TotalUsers = await _context.Users.CountAsync();

            response.TotalBookings = await _context.Bookings.CountAsync();
            response.PendingBookings = await _context.Bookings.CountAsync(b => b.Status == "Pending");

            response.TotalReports = await _context.Reports.CountAsync();
            response.PendingReports = await _context.Reports.CountAsync(b => b.Status == "Pending");

            response.TopFacilities = await _context.Bookings
                .Where(b => b.Facility != null)
                .GroupBy(b => new { b.FacilityId, b.Facility.FacilityName })
                .Select(g => new TopFacilityDto
                {
                    FacilityName = g.Key.FacilityName,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToListAsync();

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);

            var rawData = await _context.Bookings
                .Where(b => b.BookingDate >= DateOnly.FromDateTime(sixMonthsAgo))
                .Select(b => b.BookingDate)
                .ToListAsync();

            response.BookingInMonths = rawData
                .GroupBy(d => new { d.Year, d.Month })
                .Select(g => new MonthlyBookingDto
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            return response;
        }
    }
}