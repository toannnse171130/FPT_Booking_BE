using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly FptFacilityBookingContext _context;

        public SemesterRepository(FptFacilityBookingContext context)
        {
            _context = context;
        }

        public async Task<List<Semester>> GetAllSemestersAsync()
        {
            return await _context.Semesters
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
        }

        public async Task<Semester?> GetSemesterByIdAsync(int id)
        {
            return await _context.Semesters.FindAsync(id);
        }

        public async Task<Semester?> GetCurrentSemesterAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.Semesters
                .Where(s => s.IsActive && s.StartDate <= today && s.EndDate >= today)
                .FirstOrDefaultAsync();
        }

        public async Task<Semester> CreateSemesterAsync(Semester semester)
        {
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();
            return semester;
        }

        public async Task<Semester?> UpdateSemesterAsync(int id, Semester semester)
        {
            var existing = await _context.Semesters.FindAsync(id);
            if (existing == null) return null;

            existing.Name = semester.Name;
            existing.StartDate = semester.StartDate;
            existing.EndDate = semester.EndDate;
            existing.IsActive = semester.IsActive;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteSemesterAsync(int id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null) return false;

            _context.Semesters.Remove(semester);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDateInAnySemesterAsync(DateOnly date)
        {
            return await _context.Semesters
                .AnyAsync(s => s.StartDate <= date && s.EndDate >= date);
        }

        public async Task<Semester?> GetSemesterByDateAsync(DateOnly date)
        {
            return await _context.Semesters
                .Where(s => s.StartDate <= date && s.EndDate >= date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Semester>> GetSemestersInRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await _context.Semesters
                .Where(s => s.EndDate >= startDate && s.StartDate <= endDate)
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }
    }
}
