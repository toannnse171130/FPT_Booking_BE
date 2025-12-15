using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public interface ICampusService
    {
        Task<List<CampusDto>> GetAllCampuses();

        Task<Campus?> GetCampusByIdAsync(int id);

        Task<Campus> CreateCampusAsync(CampusDto request);

        Task<bool> UpdateCampusAsync(int id, CampusDto request);

        Task<bool> DeleteCampusAsync(int id);
    }
}