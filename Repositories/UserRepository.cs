using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FptFacilityBookingContext _context;

        public UserRepository(FptFacilityBookingContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
       => await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task<bool> AnyAsync(string email) => await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<(List<User> Items, int Total)> GetUsersPagedAsync(string? keyword, int? roleId, int pageIndex, int pageSize)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(u => u.FullName.Contains(keyword) || u.Email.Contains(keyword));

            if (roleId.HasValue)
                query = query.Where(u => u.RoleId == roleId);

            query = query.OrderByDescending(u => u.IsActive).ThenBy(u => u.UserId);

            int total = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}