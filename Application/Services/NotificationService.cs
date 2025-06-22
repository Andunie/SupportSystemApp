using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Dtos;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Domain.Entities;
using SupportSystemApp.Hubs;
using System.Net.Sockets;

namespace SupportSystemApp.Application.Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ITicketRepository _ticketRepository;

        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext, ITicketRepository ticketRepository)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _ticketRepository=ticketRepository;
        }

        public async Task NotifyAllAdminAsync(Guid senderUserId ,string message)
        {
            var adminUserIds = await _notificationRepository.GetAllAdminUserIdsAsync();

            foreach (var adminUserId in adminUserIds)
            {
                var notification = new Notification
                {
                    UserID = adminUserId,
                    SenderUserID = senderUserId,
                    Message = message,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    ReceiverRole = "Admin"
                };

                await _notificationRepository.AddAsync(notification);
                await _notificationRepository.SaveChangesAsync();

                var notificationDto = new NotificationDto
                {
                    Id = notification.Id,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt.ToString("g"),
                    SenderUserId = notification.SenderUserID,
                    NotificationCount = await _notificationRepository.GetUnreadCountAsync(adminUserId),
                    IsRead = false
                };

                await _hubContext.Clients.Group(adminUserId.ToString())
                    .SendAsync("AdminReceiveNotification", notificationDto);

                Console.WriteLine(">> Bildirim gönderilecek admin ID: " + adminUserId);
            }
        }

        public async Task NotifyUserAsync(Guid receiverUserId, Guid senderUserId, string message)
        {
            var notification = new Notification
            {
                UserID = receiverUserId,        // User
                SenderUserID = senderUserId,    // Admin
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                ReceiverRole = "User"
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            var notificationDto = new NotificationDto
            {
                Id = notification.Id,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt.ToString("g"),
                SenderUserId = notification.SenderUserID,
                NotificationCount = await _notificationRepository.GetUnreadCountAsync(receiverUserId),
                IsRead = false
            };

            await _hubContext.Clients.Group(receiverUserId.ToString())
                .SendAsync("ReceiveNotification", notificationDto);

            Console.WriteLine(">> Bildirim gönderildi - Receiver: " + receiverUserId);
        }

		public async Task NotifyUserMessageAsync(Guid receiverUserId, Guid senderUserId, string message, int? ticketId = null)
		{
			var notification = new Notification
			{
				UserID = receiverUserId,
				SenderUserID = senderUserId,
				Message = message,
				IsRead = false,
				CreatedAt = DateTime.UtcNow,
				ReceiverRole = "User",
				TicketId = ticketId
			};

			await _notificationRepository.AddAsync(notification);
			await _notificationRepository.SaveChangesAsync();

			Ticket? ticket = null;
			if (ticketId.HasValue)
				ticket = await _ticketRepository.GetByIdAsync(ticketId.Value);

			var notificationDto = new NotificationDto
			{
				Id = notification.Id,
				Message = notification.Message,
				CreatedAt = notification.CreatedAt.ToString("g"),
				SenderUserId = notification.SenderUserID,
				NotificationCount = await _notificationRepository.GetUnreadCountAsync(receiverUserId),
				IsRead = false,
				TicketTitle = ticket?.Title
			};

			await _hubContext.Clients.Group(receiverUserId.ToString())
				.SendAsync("ReceiveNotification", notificationDto);

			Console.WriteLine(">> Mesaj bildirimi gönderildi - Receiver: " + receiverUserId);
		}

		public async Task NotifyAdminUserMessageAsync(Guid senderUserId, Guid receiverUserId, string message, int? ticketId = null)
		{
			var notification = new Notification
			{
				UserID = receiverUserId,
				SenderUserID = senderUserId,
				Message = message,
				IsRead = false,
				CreatedAt = DateTime.UtcNow,
				ReceiverRole = "Admin",
				TicketId = ticketId
			};

			await _notificationRepository.AddAsync(notification);
			await _notificationRepository.SaveChangesAsync();

			Ticket? ticket = null;
			if (ticketId.HasValue)
				ticket = await _ticketRepository.GetByIdAsync(ticketId.Value);

			var notificationDto = new NotificationDto
			{
				Id = notification.Id,
				Message = notification.Message,
				CreatedAt = notification.CreatedAt.ToString("g"),
				SenderUserId = notification.SenderUserID,
				NotificationCount = await _notificationRepository.GetUnreadCountAsync(receiverUserId),
				IsRead = false,
				TicketTitle = ticket?.Title
			};

			await _hubContext.Clients.Group(receiverUserId.ToString())
				.SendAsync("ReceiveNotification", notificationDto);

			Console.WriteLine(">> Mesaj bildirimi gönderildi - Receiver (admin): " + receiverUserId);
		}
	}
}