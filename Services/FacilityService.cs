using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepo;
        public FacilityService(IFacilityRepository facilityRepo)
        {
            _facilityRepo = facilityRepo;
        }

        public async Task<List<FacilityDto>> GetAllFacilities(string? name, int? campusId, int? typeId, int? slotId, DateOnly? date)
        {
            var facilities = await _facilityRepo.GetAllFacilitiesAsync(name, campusId, typeId, slotId, date);

            return facilities.Select(f => new FacilityDto
            {
                FacilityId = f.FacilityId,
                FacilityName = f.FacilityName,
                FacilityCapacity = f.Capacity,
                CampusName = f.Campus?.CampusName ?? "N/A",
                TypeName = f.Type?.TypeName ?? "N/A",
                ImageUrl = f.ImageUrl ?? "",
                Status = f.Status
            }).ToList();
        }

        public async Task<string> CreateFacility(FacilityCreateRequest request)
        {
            if (await _facilityRepo.CheckNameExistsAsync(request.FacilityName))
                return "Tên phòng này đã tồn tại!";

            var newFacility = new Facility
            {
                FacilityName = request.FacilityName,
                CampusId = request.CampusId,
                TypeId = request.TypeId,
                ImageUrl = request.ImageUrl,
                Status = request.Status,
                CreatedAt = DateTime.Now
            };

            await _facilityRepo.AddAsync(newFacility);
            return "Success";
        }

        public async Task<string> UpdateFacility(int id, FacilityUpdateRequest request)
        {
            var facility = await _facilityRepo.GetByIdAsync(id);
            if (facility == null) return "Không tìm thấy phòng.";

            facility.FacilityName = request.FacilityName;
            facility.CampusId = request.CampusId;
            facility.TypeId = request.TypeId;
            if (!string.IsNullOrEmpty(request.ImageUrl)) facility.ImageUrl = request.ImageUrl;
            facility.Status = request.Status;

            await _facilityRepo.UpdateAsync(facility);
            return "Success";
        }

        public async Task<string> DeleteFacility(int id)
        {
            var facility = await _facilityRepo.GetByIdAsync(id);
            if (facility == null) return "Không tìm thấy phòng.";

            facility.Status = "Disabled"; 
            await _facilityRepo.UpdateAsync(facility);
            return "Success";
        }



    }
}