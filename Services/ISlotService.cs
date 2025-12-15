using FPT_Booking_BE.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public interface ISlotService
    {
        Task<List<SlotDto>> GetAllSlots();
    }
}