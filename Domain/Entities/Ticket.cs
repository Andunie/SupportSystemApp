using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SupportSystem.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public Guid CreatedByUserId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]  
        public bool IsResolved { get; set; }
        public string? AdminResponse { get; set; }
        public string? ImagePath { get; set; }
        public string? Priorty { get; set; } = "Unknown"; // "Acil", "Orta", "Düşük"
    }
}
