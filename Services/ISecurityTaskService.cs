using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface ISecurityTaskService
    {
        Task CreateTaskAsync(SecurityTask task);

        Task<IEnumerable<SecurityTaskDto>> GetPendingTasksAsync();

        Task<IEnumerable<SecurityTaskDto>> GetAllTasksAsync();

        Task<bool> CompleteTaskAsync(int taskId, string reportNote);
    }
}