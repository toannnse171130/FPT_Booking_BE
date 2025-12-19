using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IFacilityService
    {
        Task<List<FacilityDto>> GetAllFacilities(string? name, int? campusId, int? typeId, int? slotId, DateOnly? date);
        Task<string> CreateFacility(FacilityCreateRequest request);
        Task<string> UpdateFacility(int id, FacilityUpdateRequest request);
        Task<string> DeleteFacility(int id);
    }
}