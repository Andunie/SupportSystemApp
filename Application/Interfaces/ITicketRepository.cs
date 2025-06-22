using SupportSystem.Domain.Entities;

namespace SupportSystemApp.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> GetByIdAsync(int ticketId);
        Task<List<Ticket>> GetAllByUserIdAsync(Guid userId);
        Task<Ticket?> GetByIdAndUserIdAsync(int ticketId, Guid userId);
        Task UpdateAsync(Ticket ticket);
        Task AddAsync(Ticket ticket);
        Task SaveChangesAsync();
    }
}
