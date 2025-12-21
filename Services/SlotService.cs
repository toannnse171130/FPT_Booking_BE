using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepo;

        public SlotService(ISlotRepository slotRepo)
        {
            _slotRepo = slotRepo;
        }

        public async Task<List<SlotDto>> GetAllSlots(int? facilityId, DateOnly? date)
        {
            var slots = await _slotRepo.GetAvailableSlotsAsync(facilityId, date);

            return slots.Select(s => new SlotDto
            {
                SlotId = s.SlotId,
                SlotName = s.SlotName,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }).ToList();
        }
    }
}