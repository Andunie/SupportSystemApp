using Microsoft.AspNetCore.SignalR;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Domain.Entities;
using SupportSystemApp.Hubs;

namespace SupportSystemApp.Application.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<TicketChatHub> _hubContext;

        public MessageService(IMessageRepository messageRepository, IHubContext<TicketChatHub> hubContext)
        {
            _messageRepository = messageRepository;
            _hubContext = hubContext;
        }

        public async Task<List<TicketMessage>> GetMessagesByTicketIdAsync(int ticketId)
        {
            return await _messageRepository.GetMessagesByTicketIdAsync(ticketId);
        }

        public async Task AddMessageAsync(TicketMessage message)
        {
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
        }
    }
}