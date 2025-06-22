using SupportSystemApp.Domain.Entities;

namespace SupportSystemApp.Application.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<TicketMessage>> GetMessagesByTicketIdAsync(int ticketId);
        Task AddAsync(TicketMessage message);
        Task SaveChangesAsync();
    }
}
