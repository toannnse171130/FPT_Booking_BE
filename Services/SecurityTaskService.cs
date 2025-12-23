using FPT_Booking_BE.DTOs;
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

        public async Task<IEnumerable<SecurityTaskDto>> GetPendingTasksAsync()
        {
            var tasks = await _context.SecurityTasks
                .Include(t => t.Booking)
                .Where(t => t.Status != "Completed")
                .OrderByDescending(t => t.Priority) 
                .ThenBy(t => t.CreatedAt)
                .ToListAsync();

            return await MapTasksToDtos(tasks);
        }

        public async Task<IEnumerable<SecurityTaskDto>> GetAllTasksAsync()
        {
            var tasks = await _context.SecurityTasks
                .Include(t => t.Booking)         
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return await MapTasksToDtos(tasks);
        }

        private async Task<List<SecurityTaskDto>> MapTasksToDtos(List<SecurityTask> tasks)
        {
            var result = new List<SecurityTaskDto>();

            foreach (var task in tasks)
            {
                string? assignedToUserName = null;
                string? createdByUserName = null;

                if (task.AssignedToUserId.HasValue)
                {
                    var assignedUser = await _context.Users.FindAsync(task.AssignedToUserId.Value);
                    assignedToUserName = assignedUser?.FullName ?? "Unknown";
                }

                var createdByUser = await _context.Users.FindAsync(task.CreatedBy);
                createdByUserName = createdByUser?.FullName ?? "Unknown";
                result.Add(new SecurityTaskDto
                {
                    TaskId = task.TaskId,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Priority = task.Priority,
                    AssignedToUserId = task.AssignedToUserId,
                    AssignedToUserName = assignedToUserName,
                    CreatedBy = task.CreatedBy,
                    CreatedByUserName = createdByUserName,
                    CreatedAt = task.CreatedAt,
                    CompletedAt = task.CompletedAt,
                    ReportNote = task.ReportNote,
                    BookingId = task.BookingId,
                    SlotId = task.Booking?.SlotId,
                    FacilityName = task.Booking?.Facility?.FacilityName ?? "N/A" 
                });
            }

            return result;
        }

        public async Task<bool> CompleteTaskAsync(int taskId, string reportNote)
        {
            var task = await _context.SecurityTasks
                .Include(t => t.Booking)
                .Include(t => t.Booking.Facility)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);
            if (task == null) return false;
            if (task.TaskType == "Check-in" && task.Booking != null && reportNote != "No")
            {
                task.Booking.Status = "Checked-In";
                CreateTaskAsync(new SecurityTask
                {
                    Title = $"Theo dõi Check-out cho đặt chỗ {task.Booking.Facility?.FacilityName}",
                    Description = $"Theo dõi việc check-out cho đặt chỗ {task.Booking.Facility?.FacilityName} slot {task.Booking.SlotId}",
                    Status = "Pending",
                    Priority = "Medium",
                    TaskType = "Check-out",
                    AssignedToUserId = task.AssignedToUserId,
                    BookingId = task.BookingId,
                    CreatedBy = task.CreatedBy,
                    CreatedAt = DateTime.Now
                }).Wait();
            }
            if (task.TaskType == "Check-out" && task.Booking != null)
            {
                task.Booking.Status = "Completed";
            }

            task.Status = "Completed";
            task.CompletedAt = DateTime.Now;
            task.ReportNote = reportNote; 

            await _context.SaveChangesAsync();

            // Send notification to the booking creator
            if (task.CreatedBy > 0)
            {
                await _notiService.SendNotification(
                    task.CreatedBy,
                    "Nhiệm vụ đã hoàn thành",
                    $"Nhiệm vụ '{task.Title}' đã được hoàn thành bởi bảo vệ."
                );
            }

            return true;
        }

        public async Task<IEnumerable<SecurityStaffDto>> GetSecurityStaffWithTaskCountsAsync()
        {
            // Get Security role ID (assuming it's 6)
            const int SECURITY_ROLE_ID = 6;

            var securityStaff = await _context.Users
                .Where(u => u.RoleId == SECURITY_ROLE_ID)
                .Select(u => new SecurityStaffDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsActive = u.IsActive ?? false,
                    PendingTaskCount = _context.SecurityTasks
                        .Count(t => t.AssignedToUserId == u.UserId && t.Status != "Completed")
                })
                .OrderBy(s => s.PendingTaskCount)
                .ThenBy(s => s.FullName)
                .ToListAsync();

            return securityStaff;
        }
    }
}