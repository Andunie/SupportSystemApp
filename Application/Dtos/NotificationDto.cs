namespace SupportSystemApp.Application.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
        public Guid SenderUserId { get; set; }
        public int NotificationCount { get; set; }
        public bool IsRead { get; set; }
        public string TicketTitle { get; set; }
    }
}