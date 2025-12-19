using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IUserService
    {
        Task<PagedResult<UserResponse>> GetUsersFilterAsync(UserFilterRequest request);
        Task<bool> CreateUserAsync(CreateUserRequest request);
        Task<bool> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<bool> UpdateUserRoleAsync(int userId, int newRoleId);
        Task<bool> DeleteUserAsync(int userId);
    }
}