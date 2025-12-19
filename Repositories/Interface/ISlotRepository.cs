using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface ISlotRepository
    {
        Task<IEnumerable<Slot>> GetAllSlots();
    }
}