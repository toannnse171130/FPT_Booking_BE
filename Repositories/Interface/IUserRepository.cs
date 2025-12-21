using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<(List<User> Items, int Total)> GetUsersPagedAsync(string? keyword, int? roleId, int pageIndex, int pageSize);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> AnyAsync(string email);
        Task CreateUserAsync(User user);
    }
}