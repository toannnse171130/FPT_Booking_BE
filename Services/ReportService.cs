using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Services
{
    public class ReportService : IReportService
    {
        private readonly FptFacilityBookingContext _context;

        public ReportService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<string> CreateReport(int userId, ReportCreateRequest request)
        {
            var newReport = new Report
            {
                UserId = userId,
                FacilityId = request.FacilityId,
                BookingId = request.BookingId,
                Title = request.Title,
                Description = request.Description,
                ReportType = request.ReportType,
                Status = "Pending", 
                CreatedAt = DateTime.Now
            };

            _context.Reports.Add(newReport);
            await _context.SaveChangesAsync();
            return "Success";
        }

        public async Task<List<ReportDto>> GetReports(int? userId, string role)
        {
            var query = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Facility)
                .AsQueryable();

            // Nếu là Student/Lecturer/Security -> Chỉ xem report của chính mình
            if (role == "Student" || role == "Lecturer" || role == "Security")
            {
                query = query.Where(r => r.UserId == userId);
            }
            // Nếu là Admin/Manager -> Xem được tất cả (không lọc theo userId)

            return await query
                .Select(r => new ReportDto
                {
                    ReportId = r.ReportId,
                    Title = r.Title,
                    Description = r.Description,
                    ReportType = r.ReportType,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.User.Email,
                    FacilityName = r.Facility != null ? r.Facility.FacilityName : "N/A"
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(); 
        }

        public async Task<bool> UpdateReportStatus(int reportId, string status)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return false;

            report.Status = status;
            report.ResolvedAt = DateTime.Now; 

            await _context.SaveChangesAsync();
            return true;
        }
    }
}