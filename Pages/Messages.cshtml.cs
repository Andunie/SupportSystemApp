using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Application.Services;
using SupportSystemApp.Domain.Entities;
using System.Security.Claims;

namespace SupportSystemApp.Pages
{
    public class MessagesModel : PageModel
    {
        private readonly MessageService _messageService;
        private readonly CookieAuthService _cookieAuthService;
        private readonly ITicketRepository _ticketRepository;
        private readonly NotificationService _notificationService;

        public MessagesModel(MessageService messageService, CookieAuthService cookieAuthService, ITicketRepository ticketRepository, NotificationService notificationService)
        {
            _messageService = messageService;
            _cookieAuthService = cookieAuthService;
            _ticketRepository = ticketRepository;
            _notificationService = notificationService;
        }

        public List<TicketMessage> Messages { get; set; }

        // Query string'den ticketId alýnacak
        [BindProperty(SupportsGet = true)]
        public int TicketId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ticket == null || ticket.CreatedByUserId != currentUserId)
            {
                return Unauthorized(); // veya RedirectToPage("/AccessDenied")
            }

            TicketId = id;
            Messages = await _messageService.GetMessagesByTicketIdAsync(id);
            return Page();
        }
    }
}
