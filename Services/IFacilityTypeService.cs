using FPT_Booking_BE.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public interface IFacilityTypeService
    {
        Task<List<FacilityTypeDto>> GetAllTypes();
        Task<string> CreateType(string typeName);
        Task<string> DeleteType(int id);
    }
}