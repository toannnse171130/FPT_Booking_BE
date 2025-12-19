using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface INotificationRepository
    {
        Task AddNotification(Notification notification);
        Task<IEnumerable<Notification>> GetMyNotifications(int userId);
        Task MarkAsRead(int notificationId);
    }
}

