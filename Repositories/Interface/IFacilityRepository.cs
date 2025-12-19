using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface IFacilityRepository
    {
        Task<IEnumerable<Facility>> GetFacilities(int? campusId, int? typeId);
        Task<Facility> GetFacilityById(int id);
    }
}