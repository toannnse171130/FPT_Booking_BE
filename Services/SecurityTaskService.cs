using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Services
{
    public class SecurityTaskService : ISecurityTaskService
    {
        private readonly FptFacilityBookingContext _context;
        private readonly INotificationService _notiService; 

        public SecurityTaskService(FptFacilityBookingContext context, INotificationService notiService)
        {
            _context = context;
            _notiService = notiService;
        }

        public async Task CreateTaskAsync(SecurityTask task)
        {
            _context.SecurityTasks.Add(task);
            await _context.SaveChangesAsync();

            if (task.AssignedToUserId.HasValue)
            {
                await _notiService.SendNotification(
                    task.AssignedToUserId.Value,
                    "Nhiệm vụ mới",
                    $"Bạn có nhiệm vụ mới: {task.Title}"
                );
            }
        }

        public async Task<IEnumerable<SecurityTask>> GetPendingTasksAsync()
        {
            return await _context.SecurityTasks
                .Where(t => t.Status != "Completed")
                .OrderByDescending(t => t.Priority) 
                .ThenBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> CompleteTaskAsync(int taskId, string reportNote)
        {
            var task = await _context.SecurityTasks.FindAsync(taskId);
            if (task == null) return false;

            task.Status = "Completed";
            task.CompletedAt = DateTime.Now;
            task.ReportNote = reportNote; 

            await _context.SaveChangesAsync();
            return true;
        }
    }
}