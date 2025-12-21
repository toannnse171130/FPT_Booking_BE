using FPT_Booking_BE.DTOs;
using FPT_Booking_BE.Models;
using FPT_Booking_BE.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FPT_Booking_BE.Services
{
    public class SecurityTaskService : ISecurityTaskService
    {
        private readonly ISecurityTaskRepository _taskRepo;
        private readonly INotificationService _notiService;

        public SecurityTaskService(ISecurityTaskRepository taskRepo, INotificationService notiService)
        {
            _taskRepo = taskRepo;
            _notiService = notiService;
        }

        public async Task CreateTaskAsync(SecurityTask task)
        {
            await _taskRepo.AddAsync(task);

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
            var tasks = await _taskRepo.GetTasksAsync(onlyPending: true);
            return MapToDtoList(tasks);
        }

        public async Task<IEnumerable<SecurityTaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepo.GetTasksAsync(onlyPending: false);
            return MapToDtoList(tasks);
        }

        public async Task<bool> CompleteTaskAsync(int taskId, string reportNote)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) return false;

            task.Status = "Completed";
            task.CompletedAt = DateTime.Now;
            task.ReportNote = reportNote;

            await _taskRepo.UpdateAsync(task);
            return true;
        }

        private IEnumerable<SecurityTaskDto> MapToDtoList(List<SecurityTask> tasks)
        {
            return tasks.Select(task => new SecurityTaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUserName = task.AssignedToUser?.FullName ?? "Unknown",
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt,
                ReportNote = task.ReportNote,
                BookingId = task.BookingId,
                SlotId = task.Booking?.SlotId,
                FacilityName = task.Booking?.Facility?.FacilityName ?? "N/A"
            });
        }
    }
}