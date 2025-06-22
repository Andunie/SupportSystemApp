using Microsoft.AspNetCore.Mvc;

namespace SupportSystemApp.Application.Dtos
{
	// For listing
	public class TicketDto 
	{
        public int Id { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
		public bool IsResolved { get; set; }
		public DateTime CreatedAt { get; set; }
        public string? AdminResponse { get; set; }
        public string? AiResponse { get; set; }
        public string? ImagePath { get; set; }
    }
}
