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
    }
}