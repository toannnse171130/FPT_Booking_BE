using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class SlotService : ISlotService
    {
        private readonly FptFacilityBookingContext _context;

        public SlotService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<SlotDto>> GetAllSlots()
        {
            var slots = await _context.Slots
                .Where(s => s.IsActive == true)
                .Select(s => new SlotDto
                {
                    SlotId = s.SlotId,
                    SlotName = s.SlotName,
                    StartTime = s.StartTime, 
                    EndTime = s.EndTime,
                    IsActive = s.IsActive ?? false
                })
                .OrderBy(s => s.SlotName)
                .ToListAsync();

            return slots;
        }
    }
}