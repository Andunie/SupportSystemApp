using SupportSystemApp.Domain.Entities;

namespace SupportSystemApp.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllByUserAsync(Guid userId);
        Task AddAsync(Notification notification);
        Task MarkAsReadAsync(int id);
        Task SaveChangesAsync();
        Task<List<Guid>> GetAllAdminUserIdsAsync();
        Task<int> GetUnreadCountAsync(Guid userId);
    }
}
