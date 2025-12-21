using FPT_Booking_BE.DTOs; 
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class CampusService : ICampusService
    {
        private readonly ICampusRepository _campusRepo;
        public CampusService(ICampusRepository campusRepo)
        {
            _campusRepo = campusRepo;
        }

        public async Task<List<CampusDto>> GetAllCampuses()
        {
            var campuses = await _campusRepo.GetAllActiveCampusesAsync();
            return campuses.Select(c => new CampusDto
            {
                CampusId = c.CampusId,
                CampusName = c.CampusName,
                Address = c.Address, 
                IsActive = c.IsActive
            }).ToList();
        }

        public async Task<Campus?> GetCampusByIdAsync(int id)
        {
            return await _campusRepo.GetByIdAsync(id);
        }

        public async Task<Campus> CreateCampusAsync(CampusDto request)
        {
            var campus = new Campus
            {
                CampusName = request.CampusName,
                Address = request.Address,
                IsActive = true
            };

            await _campusRepo.CreateAsync(campus);
            return campus;
        }

        public async Task<bool> UpdateCampusAsync(int id, CampusDto request)
        {
            var existing = await _campusRepo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.CampusName = request.CampusName;
            existing.Address = request.Address;
            existing.IsActive = request.IsActive;

            await _campusRepo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteCampusAsync(int id)
        {
            var campus = await _campusRepo.GetByIdAsync(id);
            if (campus == null) return false;

            campus.IsActive = false;

            await _campusRepo.UpdateAsync(campus);
            return true;
        }
    }
}