using SupportSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace SupportSystemApp.Domain.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserID { get; set; } // Bildirimi alan
        [Required]
        public Guid SenderUserID { get; set; } // Bildirimi gönderen
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public string ReceiverRole { get; set; }
		public int? TicketId { get; set; }
		public Ticket? Ticket { get; set; }

	}
}
