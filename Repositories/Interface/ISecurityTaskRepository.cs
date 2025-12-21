using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories.Interface
{
    public interface ISecurityTaskRepository
    {
        Task AddAsync(SecurityTask task);
        Task<List<SecurityTask>> GetTasksAsync(bool onlyPending);
        Task<SecurityTask?> GetByIdAsync(int id);
        Task UpdateAsync(SecurityTask task);
    }
}
