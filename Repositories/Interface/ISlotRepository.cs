using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface ISlotRepository
    {
        Task<List<Slot>> GetAvailableSlotsAsync(int? facilityId, DateOnly? date);
    }
}