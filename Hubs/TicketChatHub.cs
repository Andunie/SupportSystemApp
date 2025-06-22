using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Application.Services;
using SupportSystemApp.Domain.Entities;

namespace SupportSystemApp.Hubs
{
    public class TicketChatHub : Hub
    {
        private readonly MessageService _messageService;
        private readonly NotificationService _notificationService;
        private readonly ITicketRepository _ticketRepository;
        private readonly AppDbContext _context;

        public TicketChatHub(MessageService messageService, NotificationService notificationService, ITicketRepository ticketRepository, AppDbContext context)
        {
            _messageService = messageService;
            _notificationService = notificationService;
            _ticketRepository = ticketRepository;
            _context = context;
        }

        public async Task JoinGroup(int ticketId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        }

        public async Task SendMessage(int ticketId, string senderUserId, string message)
        {
            if (!Guid.TryParse(senderUserId, out var senderGuid))
            {
                throw new HubException("Invalid senderUserId");
            }

            var ticketMessage = new TicketMessage
            {
                TicketId = ticketId,
                SenderUserId = senderGuid,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            await _messageService.AddMessageAsync(ticketMessage);

            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

            if (ticket != null)
            {
                Guid receiverUserId = ticket.CreatedByUserId;

                if (senderGuid == receiverUserId)
                {
                    // Kullanıcı mesaj gönderdi → Admin'e bildir
                    var lastAdminMessage = await _context.TicketMessages
                        .Where(m => m.TicketId == ticketId && m.SenderUserId != receiverUserId)
                        .OrderByDescending(m => m.SentAt)
                        .FirstOrDefaultAsync();

                    if (lastAdminMessage != null)
                    {
                        Guid adminUserId = lastAdminMessage.SenderUserId;

                        await _notificationService.NotifyAdminUserMessageAsync(senderGuid, adminUserId, message, ticketId);
                    }
                }
                else
                {
                    // Admin mesaj gönderdi → Kullanıcıya bildir
                    await _notificationService.NotifyUserMessageAsync(receiverUserId, senderGuid, message, ticketId);
                }
            }

            await Clients.Group($"ticket-{ticketId}")
                .SendAsync("ReceiveMessage", senderUserId, message, ticketMessage.SentAt.ToString("yyyy-MM-dd HH:mm"));
        }
    }
}
