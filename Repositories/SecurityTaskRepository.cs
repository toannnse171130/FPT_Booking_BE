using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Repositories
{
    public class SecurityTaskRepository : ISecurityTaskRepository
    {
        private readonly FptFacilityBookingContext _context;
        public SecurityTaskRepository(FptFacilityBookingContext context) => _context = context;

        public async Task AddAsync(SecurityTask task)
        {
            await _context.SecurityTasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SecurityTask>> GetTasksAsync(bool onlyPending)
        {
            var query = _context.SecurityTasks
                .Include(t => t.Booking).ThenInclude(b => b.Facility)
                .Include(t => t.AssignedToUser)
                .AsQueryable();

            if (onlyPending)
            {
                query = query.Where(t => t.Status != "Completed")
                             .OrderByDescending(t => t.Priority);
            }
            else
            {
                query = query.OrderByDescending(t => t.CreatedAt);
            }

            return await query.ToListAsync();
        }

        public async Task<SecurityTask?> GetByIdAsync(int id) => await _context.SecurityTasks.FindAsync(id);

        public async Task UpdateAsync(SecurityTask task)
        {
            _context.SecurityTasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
