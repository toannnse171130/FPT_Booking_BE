using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task CreateUserAsync(User user);
    }
}