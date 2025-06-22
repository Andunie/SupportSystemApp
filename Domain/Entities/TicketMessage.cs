using SupportSystem.Domain.Entities;

namespace SupportSystemApp.Domain.Entities
{
    public class TicketMessage
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Guid SenderUserId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public Ticket Ticket { get; set; }
        public User SenderUser { get; set; }
    }
}