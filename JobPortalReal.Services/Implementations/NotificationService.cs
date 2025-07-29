using JobPortalReal.DataAccess.Data;
using JobPortalReal.Models;
using JobPortalReal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace JobPortalReal.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly JobPortalContext _context;
        public NotificationService(JobPortalContext context)
        {
            _context = context;
        }
        // Add a new notification
        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
        // Get notifications for a specific user
        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId)
        {
            var notifications = await _context.Notifications
                   .Where(n => n.UserId == userId)
                   .OrderByDescending(n => n.CreatedDate) // Latest notifications first
                   .ToListAsync();

            return notifications;
        }
        // Mark a notification as read
        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }
        public async Task DeleteNotificationAsync(Notification notification)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            var notifyCount = _context.Notifications.Where(n => n.UserId == userId && n.IsRead == false).Count();
            return Task.FromResult(notifyCount);
        }
    }
}
