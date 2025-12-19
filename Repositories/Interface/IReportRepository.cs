using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface IReportRepository
    {
        Task AddReport(Report report);
        Task<IEnumerable<Report>> GetReports(int? userId, string? status); 
        Task<Report?> GetReportById(int id);
        Task UpdateReport(Report report);
    }
}
