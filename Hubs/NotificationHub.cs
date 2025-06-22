using Microsoft.AspNetCore.SignalR;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Application.Dtos;
using System.Security.Claims;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;

namespace SupportSystemApp.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ITicketRepository _ticketRepository;

        public NotificationHub(INotificationRepository notificationRepository, ITicketRepository ticketRepository)
        {
            _notificationRepository = notificationRepository;
            _ticketRepository = ticketRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine(">> SignalR: Kullanıcı gruba eklendi => " + userId);
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            else
            {
                Console.WriteLine(">> SignalR: UserIdentifier boş.");
            }

            await base.OnConnectedAsync();
        }

        // Client'tan bildirimi okundu olarak işaretlemek için metod
        public async Task MarkNotificationAsRead(int notificationId)
        {
            await _notificationRepository.MarkAsReadAsync(notificationId);
            await _notificationRepository.SaveChangesAsync();
        }

        // Okunmamış bildirim sayısını almak için metod
        public async Task<int> GetUnreadNotificationCount()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return 0;
            }

            return await _notificationRepository.GetUnreadCountAsync(userGuid);
        }

		public async Task<List<NotificationDto>> GetAllNotifications()
		{
			var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
			{
				return new List<NotificationDto>();
			}

			// Notification ile birlikte Ticket verisini de çek (eager loading)
			var notifications = await _notificationRepository.GetAllByUserAsync(userGuid);

			return notifications.Select(n => new NotificationDto
			{
				Id = n.Id,
				Message = n.Message,
				CreatedAt = n.CreatedAt.ToLocalTime().ToString("g"),
				SenderUserId = n.SenderUserID,
				IsRead = n.IsRead,
				NotificationCount = notifications.Count(x => !x.IsRead),
				TicketTitle = n.Ticket?.Title // null olabilir, bu yüzden güvenli erişim
			})
			.OrderByDescending(n => n.CreatedAt)
			.ToList();
		}
	}
}
