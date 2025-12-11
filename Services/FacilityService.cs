using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models; 
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly FptFacilityBookingContext _context;

        public FacilityService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<FacilityDto>> GetAllFacilities(string? name, int? campusId, int? typeId)
        {
            var query = _context.Facilities
                .Include(f => f.Campus)
                .Include(f => f.Type)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(f => f.FacilityName.Contains(name));
            }

            if (campusId.HasValue)
            {
                query = query.Where(f => f.CampusId == campusId);
            }

            if (typeId.HasValue)
            {
                query = query.Where(f => f.TypeId == typeId);
            }

            return await query
                .Select(f => new FacilityDto
                {
                    FacilityId = f.FacilityId,
                    FacilityName = f.FacilityName,
                    CampusName = f.Campus.CampusName,
                    TypeName = f.Type.TypeName,
                    ImageUrl = f.ImageUrl ?? "",
                    Status = f.Status
                })
                .ToListAsync();
        }

        public async Task<string> CreateFacility(FacilityCreateRequest request)
        {
            var exists = await _context.Facilities.AnyAsync(f => f.FacilityName == request.FacilityName);
            if (exists) return "Tên phòng này đã tồn tại!";

            var newFacility = new Facility
            {
                FacilityName = request.FacilityName,
                CampusId = request.CampusId,
                TypeId = request.TypeId,
                ImageUrl = request.ImageUrl,
                Status = request.Status,
                CreatedAt = DateTime.Now
            };

            _context.Facilities.Add(newFacility);
            await _context.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> UpdateFacility(int id, FacilityUpdateRequest request)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return "Không tìm thấy phòng.";

            facility.FacilityName = request.FacilityName;
            facility.CampusId = request.CampusId;
            facility.TypeId = request.TypeId;

            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                facility.ImageUrl = request.ImageUrl;
            }

            facility.Status = request.Status;

            await _context.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> DeleteFacility(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return "Không tìm thấy phòng.";

            facility.Status = "Disabled";

            await _context.SaveChangesAsync();
            return "Success";
        }



    }
}