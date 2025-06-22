using Microsoft.EntityFrameworkCore;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Interfaces;

namespace SupportSystemApp.Infrastructre.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Tickets
                .Where(t => t.CreatedByUserId == userId)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAndUserIdAsync(int ticketId, Guid userId)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == ticketId && t.CreatedByUserId == userId);
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket> GetByIdAsync(int ticketId)
        {
            return await _context.Tickets.FindAsync(ticketId);
        }
    }
}
