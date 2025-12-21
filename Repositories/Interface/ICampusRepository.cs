using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Repositories
{
    public interface ICampusRepository
    {
        Task<List<Campus>> GetAllActiveCampusesAsync();
        Task<Campus?> GetByIdAsync(int id);
        Task CreateAsync(Campus campus);
        Task UpdateAsync(Campus campus);
    }
}