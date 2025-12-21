using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly FptFacilityBookingContext _context;

        public SlotRepository(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<Slot>> GetAvailableSlotsAsync(int? facilityId, DateOnly? date)
        {
            var query = _context.Slots.AsQueryable();

            if (facilityId.HasValue && date.HasValue)
            {
                query = query.Where(s => !_context.Bookings.Any(b =>
                    b.SlotId == s.SlotId &&
                    b.FacilityId == facilityId &&
                    b.BookingDate == date &&
                    b.Status == "Approved"
                ));
            }

            return await query.ToListAsync();
        }
    }
}