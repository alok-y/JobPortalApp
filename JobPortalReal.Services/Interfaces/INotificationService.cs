using JobPortalReal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace JobPortalReal.Services.Interfaces
{
    public interface INotificationService
    {
        // Add a notification
        Task AddNotificationAsync(Notification notification);
        // Get notifications for a specific user
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId);
        // Mark a notification as read
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
        // Delete a notification
        Task DeleteNotificationAsync(Notification notification);
        // Get a notification by its ID
        Task<Notification> GetNotificationByIdAsync(int id);

        // isread false count
        Task<int> GetUnreadNotificationCountAsync(string userId);


    }
}