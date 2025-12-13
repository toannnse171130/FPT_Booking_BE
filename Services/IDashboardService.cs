using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsResponse> GetStatsAsync();
    }
}