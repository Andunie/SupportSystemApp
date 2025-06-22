using Microsoft.EntityFrameworkCore;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Domain.Entities;

namespace SupportSystemApp.Infrastructre.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TicketMessage message)
        {
            _context.TicketMessages.Add(message);   
        }

        public async Task<List<TicketMessage>> GetMessagesByTicketIdAsync(int ticketId)
        {
            return await _context.TicketMessages.Where(x => x.TicketId == ticketId)
                .OrderBy(m => m.SentAt).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
