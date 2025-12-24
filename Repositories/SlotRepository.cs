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

        public async Task<IEnumerable<Slot>> GetAllSlots()
        {
            return await _context.Slots.Where(s => s.IsActive == true).ToListAsync();
        }

        public async Task<Slot?> GetSlotById(int slotId)
        {
            return await _context.Slots.FindAsync(slotId);
        }
    }
}