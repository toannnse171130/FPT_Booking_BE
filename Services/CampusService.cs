using FPT_Booking_BE.DTOs; 
using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class CampusService : ICampusService
    {
        private readonly FptFacilityBookingContext _context;

        public CampusService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<CampusDto>> GetAllCampuses()
        {
            return await _context.Campuses
                .Where(c => c.IsActive == true)
                .Select(c => new CampusDto
                {
                    CampusId = c.CampusId,
                    CampusName = c.CampusName
                })
                .ToListAsync();
        }

        public async Task<Campus?> GetCampusByIdAsync(int id)
        {
            return await _context.Campuses.FindAsync(id);
        }

        public async Task<Campus> CreateCampusAsync(CampusDto request)
        {
            var campus = new Campus
            {
                CampusName = request.CampusName,
                Address = request.Address,
                IsActive = true
            };

            _context.Campuses.Add(campus);
            await _context.SaveChangesAsync();
            return campus;
        }

        public async Task<bool> UpdateCampusAsync(int id, CampusDto request)
        {
            var existing = await _context.Campuses.FindAsync(id);
            if (existing == null) return false;

            existing.CampusName = request.CampusName;
            existing.Address = request.Address;
            existing.IsActive = request.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCampusAsync(int id)
        {
            var campus = await _context.Campuses.FindAsync(id);
            if (campus == null) return false;

            campus.IsActive = false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}