using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Repositories
{
    public class CampusRepository : ICampusRepository
    {
        private readonly FptFacilityBookingContext _context;

        public CampusRepository(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<Campus>> GetAllActiveCampusesAsync()
        {
            return await _context.Campuses.Where(c => c.IsActive == true).ToListAsync();
        }

        public async Task<Campus?> GetByIdAsync(int id)
        {
            return await _context.Campuses.FindAsync(id);
        }

        public async Task CreateAsync(Campus campus)
        {
            await _context.Campuses.AddAsync(campus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Campus campus)
        {
            _context.Campuses.Update(campus);
            await _context.SaveChangesAsync();
        }
    }
}