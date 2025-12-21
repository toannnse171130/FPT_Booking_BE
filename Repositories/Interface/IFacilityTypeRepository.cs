using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories.Interface
{
    public interface IFacilityTypeRepository
    {
        Task<List<FacilityType>> GetAllAsync();
        Task<FacilityType?> GetByIdAsync(int id);
        Task AddAsync(FacilityType type);
        Task DeleteAsync(FacilityType type);
    }
}
