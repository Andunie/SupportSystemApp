using Microsoft.EntityFrameworkCore;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Domain.Entities;

namespace SupportSystemApp.Infrastructre.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task<List<Guid>> GetAllAdminUserIdsAsync()
        {
            return await _context.Users
                .Where(u => u.IsAdmin)
                .Select(u => u.Id)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetAllByUserAsync(Guid userId)
        {
            return await _context.Notifications
                .Include(n => n.Ticket)
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(n => n.UserID == userId && !n.IsRead)
                .CountAsync();
        }


        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
