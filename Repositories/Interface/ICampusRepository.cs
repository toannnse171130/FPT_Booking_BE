using FPT_Booking_BE.Models;using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Repositories
{
    public interface ICampusRepository
    {
        Task<IEnumerable<Campus>> GetAllCampuses();
    }
}