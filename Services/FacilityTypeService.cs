using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class FacilityTypeService : IFacilityTypeService
    {
        private readonly IFacilityTypeRepository _typeRepo;
        public FacilityTypeService(IFacilityTypeRepository typeRepo)
        {
            _typeRepo = typeRepo;
        }

        public async Task<List<FacilityTypeDto>> GetAllTypes()
        {
            var types = await _typeRepo.GetAllAsync();

            return types.Select(t => new FacilityTypeDto
            {
                TypeId = t.TypeId,
                TypeName = t.TypeName
            }).ToList();
        }

        public async Task<string> CreateType(string typeName)
        {
            var newType = new FacilityType { TypeName = typeName };
            await _typeRepo.AddAsync(newType);
            return "Success";
        }

        public async Task<string> DeleteType(int id)
        {
            var type = await _typeRepo.GetByIdAsync(id);
            if (type == null) return "Not found";

            await _typeRepo.DeleteAsync(type);
            return "Success";
        }
    }
}