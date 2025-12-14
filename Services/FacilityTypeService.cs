using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Booking_BE.Services
{
    public class FacilityTypeService : IFacilityTypeService
    {
        private readonly FptFacilityBookingContext _context;

        public FacilityTypeService(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<FacilityTypeDto>> GetAllTypes()
        {
            return await _context.FacilityTypes
                .Select(t => new FacilityTypeDto
                {
                    TypeId = t.TypeId,
                    TypeName = t.TypeName
                })
                .ToListAsync();
        }

        public async Task<string> CreateType(string typeName)
        {
            _context.FacilityTypes.Add(new FacilityType { TypeName = typeName });
            await _context.SaveChangesAsync();
            return "Success";
        }

        public async Task<string> DeleteType(int id)
        {
            var type = await _context.FacilityTypes.FindAsync(id);
            if (type == null) return "Not found";

            _context.FacilityTypes.Remove(type); 
            await _context.SaveChangesAsync();
            return "Success";
        }
    }
}