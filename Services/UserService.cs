using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo) => _userRepo = userRepo;

        public async Task<PagedResult<UserResponse>> GetUsersFilterAsync(UserFilterRequest request)
        {
            var (users, total) = await _userRepo.GetUsersPagedAsync(request.Keyword, request.RoleId, request.PageIndex, request.PageSize);

            return new PagedResult<UserResponse>
            {
                Items = users.Select(u => new UserResponse
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    FullName = u.FullName,
                    RoleName = u.Role?.RoleName ?? "N/A",
                    IsActive = u.IsActive ?? false
                }).ToList(),
                TotalRecords = total,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        public async Task<bool> CreateUserAsync(CreateUserRequest request)
        {
            if (await _userRepo.AnyAsync(request.Email)) return false;

            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                RoleId = request.RoleId,
                PasswordHash = request.Password,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _userRepo.AddAsync(user);
            return true;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return false;

            user.FullName = request.FullName;
            user.PhoneNumber = request.Phone;
            user.IsActive = request.IsActive;

            await _userRepo.UpdateAsync(user);
            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, int newRoleId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            user.RoleId = newRoleId;
            await _userRepo.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            await _userRepo.UpdateAsync(user);
            return true;
        }
    }
}