using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Services
{
    public interface ISecurityTaskService
    {
        Task CreateTaskAsync(SecurityTask task);

        Task<IEnumerable<SecurityTask>> GetPendingTasksAsync();

        Task<bool> CompleteTaskAsync(int taskId, string reportNote);
    }
}