using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Domain.Entities;
using SupportSystemApp.Application.Services;
using SupportSystemApp.Domain.Entities;

namespace SupportSystemApp.Pages.Admin
{
    public class TicketMessagesModel : PageModel
    {
        private readonly MessageService _messageService;

        public TicketMessagesModel(MessageService messageService)
        {
            _messageService = messageService;
        }

        // Sayfaya mesaj listesi gönderilecek
        public List<TicketMessage> Messages { get; set; }

        // Query string'den ticketId alýnacak
        [BindProperty(SupportsGet = true)]
        public int TicketId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
			TicketId = id;
            if (TicketId == null)
            {
                RedirectToPage("/AllTickets");
            }
			Messages = await _messageService.GetMessagesByTicketIdAsync(TicketId);
            return Page();
        }
    }
}
