using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Application.Services;

namespace SupportSystemApp.Pages.Admin
{
    public class RespondToTicketModel : PageModel
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly CookieAuthService _cookieAuthService;
        private readonly NotificationService _notificationService;

        public RespondToTicketModel(ITicketRepository ticketRepository, CookieAuthService cookieAuthService, NotificationService notificationService)
        {
            _ticketRepository = ticketRepository;
            _cookieAuthService = cookieAuthService;
            _notificationService = notificationService;
        }

        [BindProperty]
        public int TicketId { get; set; }

        [BindProperty]
        public string AdminResponse { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = _cookieAuthService.GetCurrentUserId();
            if (userId == null)
                return RedirectToPage("/Login");

            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return NotFound();

            TicketId = ticket.Id;
            AdminResponse = ticket.AdminResponse ?? string.Empty;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _cookieAuthService.GetCurrentUserId();
            if (userId == null)
                return RedirectToPage("/Login");

            var ticket = await _ticketRepository.GetByIdAsync(TicketId);
            if (ticket == null)
                return NotFound();

            ticket.AdminResponse = AdminResponse;
            await _ticketRepository.SaveChangesAsync();

            // Bildirim gönderimi: 
            var receiverUserId = ticket.CreatedByUserId;
            var senderUserId = userId;
            string message = $"Your ticket #{ticket.Id} has been answered by an admin";

            await _notificationService.NotifyUserAsync(receiverUserId, (Guid)senderUserId, message);

            return RedirectToPage("/Admin/AllTickets");
        }
    }
}
