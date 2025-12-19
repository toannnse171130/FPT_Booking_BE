using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Services
{
    public interface ISemesterService
    {
        Task<List<Semester>> GetAllSemestersAsync();
        Task<Semester?> GetSemesterByIdAsync(int id);
        Task<Semester?> GetCurrentSemesterAsync();
        Task<Semester> CreateSemesterAsync(Semester semester);
        Task<Semester?> UpdateSemesterAsync(int id, Semester semester);
        Task<bool> DeleteSemesterAsync(int id);
        Task<List<Semester>> GetSemestersInRangeAsync(DateOnly startDate, DateOnly endDate);
    }
}
