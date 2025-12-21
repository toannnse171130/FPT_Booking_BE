using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface IFacilityRepository
    {
        Task<List<Facility>> GetAllFacilitiesAsync(string? name, int? campusId, int? typeId, int? slotId, DateOnly? date);
        Task<Facility?> GetByIdAsync(int id);
        Task<bool> CheckNameExistsAsync(string name);
        Task AddAsync(Facility facility);
        Task UpdateAsync(Facility facility);
    }
}