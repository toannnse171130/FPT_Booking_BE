using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories;

namespace FPT_Booking_BE.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepo;

        public SemesterService(ISemesterRepository semesterRepo)
        {
            _semesterRepo = semesterRepo;
        }

        public async Task<List<Semester>> GetAllSemestersAsync()
        {
            return await _semesterRepo.GetAllSemestersAsync();
        }

        public async Task<Semester?> GetSemesterByIdAsync(int id)
        {
            return await _semesterRepo.GetSemesterByIdAsync(id);
        }

        public async Task<Semester?> GetCurrentSemesterAsync()
        {
            return await _semesterRepo.GetCurrentSemesterAsync();
        }

        public async Task<Semester> CreateSemesterAsync(Semester semester)
        {
            // Validate dates
            if (semester.StartDate >= semester.EndDate)
            {
                throw new Exception("Ngày bắt đầu phải trước ngày kết thúc");
            }

            return await _semesterRepo.CreateSemesterAsync(semester);
        }

        public async Task<Semester?> UpdateSemesterAsync(int id, Semester semester)
        {
            // Validate dates
            if (semester.StartDate >= semester.EndDate)
            {
                throw new Exception("Ngày bắt đầu phải trước ngày kết thúc");
            }

            return await _semesterRepo.UpdateSemesterAsync(id, semester);
        }

        public async Task<bool> DeleteSemesterAsync(int id)
        {
            return await _semesterRepo.DeleteSemesterAsync(id);
        }

        public async Task<List<Semester>> GetSemestersInRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Ngày bắt đầu phải trước hoặc bằng ngày kết thúc");
            }

            return await _semesterRepo.GetSemestersInRangeAsync(startDate, endDate);
        }
    }
}
