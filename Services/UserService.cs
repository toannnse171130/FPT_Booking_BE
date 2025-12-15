using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Services
{
    public class UserService : IUserService
    {
        private readonly FptFacilityBookingContext _context;

        public UserService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<UserResponse>> GetUsersFilterAsync(UserFilterRequest request)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(u => u.FullName.Contains(request.Keyword)
                                      || u.Email.Contains(request.Keyword));
            }

            if (request.RoleId.HasValue)
            {
                query = query.Where(u => u.RoleId == request.RoleId);
            }
            query = query.OrderByDescending(u => u.IsActive).ThenBy(u => u.UserId);

            int totalRecords = await query.CountAsync();

            var items = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(u => new UserResponse
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    FullName = u.FullName,
                    RoleName = u.Role != null ? u.Role.RoleName : "N/A",
                    IsActive = true
                })
                .ToListAsync();

            return new PagedResult<UserResponse>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        public async Task<bool> CreateUserAsync(CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return false;

            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                RoleId = request.RoleId,
                PasswordHash = request.Password,
                IsActive = true,
                CreatedAt = DateTime.Now 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.FullName = request.FullName;
             user.PhoneNumber = request.Phone; 
             user.IsActive = request.IsActive; 

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, int newRoleId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            user.RoleId = newRoleId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            try
            {
                user.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}