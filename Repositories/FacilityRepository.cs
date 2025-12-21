using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly FptFacilityBookingContext _context;

        public FacilityRepository(FptFacilityBookingContext context)
        {
            _context = context;
        }
        public async Task<List<Facility>> GetAllFacilitiesAsync(string? name, int? campusId, int? typeId, int? slotId, DateOnly? date)
        {
            var query = _context.Facilities
                .Include(f => f.Campus)
                .Include(f => f.Type)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(f => f.FacilityName.Contains(name));

            if (campusId.HasValue)
                query = query.Where(f => f.CampusId == campusId);

            if (typeId.HasValue)
                query = query.Where(f => f.TypeId == typeId);

            if (slotId.HasValue && date.HasValue)
            {
                query = query.Where(f => !_context.Bookings.Any(b =>
                    b.FacilityId == f.FacilityId &&
                    b.BookingDate == date &&
                    b.SlotId == slotId &&
                    b.Status == "Approved"
                ));
            }

            return await query.ToListAsync();
        }

        public async Task<Facility?> GetByIdAsync(int id) => await _context.Facilities.FindAsync(id);

        public async Task<bool> CheckNameExistsAsync(string name)
            => await _context.Facilities.AnyAsync(f => f.FacilityName == name);

        public async Task AddAsync(Facility facility)
        {
            await _context.Facilities.AddAsync(facility);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Facility facility)
        {
            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();
        }
    }
}