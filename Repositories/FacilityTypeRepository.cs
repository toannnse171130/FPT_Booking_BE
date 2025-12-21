using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Repositories
{
    public class FacilityTypeRepository : IFacilityTypeRepository
    {
        private readonly FptFacilityBookingContext _context;

        public FacilityTypeRepository(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<FacilityType>> GetAllAsync()
        {
            return await _context.FacilityTypes.ToListAsync();
        }

        public async Task<FacilityType?> GetByIdAsync(int id)
        {
            return await _context.FacilityTypes.FindAsync(id);
        }

        public async Task AddAsync(FacilityType type)
        {
            await _context.FacilityTypes.AddAsync(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FacilityType type)
        {
            _context.FacilityTypes.Remove(type);
            await _context.SaveChangesAsync();
        }
    }
}
