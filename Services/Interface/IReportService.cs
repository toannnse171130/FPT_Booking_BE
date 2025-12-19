using FPT_Booking_BE.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FPT_Booking_BE.Services
{
    public interface IReportService
    {
        Task<string> CreateReport(int userId, ReportCreateRequest request);
        Task<List<ReportDto>> GetReports(int? userId, string role);
        Task<bool> UpdateReportStatus(int reportId, string status);
    }
}