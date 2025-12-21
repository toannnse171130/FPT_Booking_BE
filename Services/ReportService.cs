using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
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

            await _repo.AddReport(newReport);
            return "Success";
        }

        public async Task<List<ReportDto>> GetReports(int? userId, string role)
        {
            var query = _repo.GetReportsQuery();

            if (role == "Student" || role == "Lecturer" || role == "Security")
            {
                query = query.Where(r => r.UserId == userId);
            }

            return await query
                .Select(r => new ReportDto
                {
                    ReportId = r.ReportId,
                    BookingId = r.BookingId,
                    UserId = r.UserId,
                    UserName = r.User.FullName,
                    SlotId = r.Booking.SlotId != null ? r.Booking.SlotId : 0,
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
            var report = await _repo.GetReportById(reportId);
            if (report == null) return false;

            report.Status = status;
            report.ResolvedAt = DateTime.Now;

            await _repo.UpdateReport(report);
            return true;
        }
    }
}