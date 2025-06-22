using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Application.Services;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Dtos;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Application.Services;

namespace SupportSystemApp.Pages
{
	public class MyTicketsModel : PageModel
	{
        private readonly ITicketRepository _ticketRepository;
        private readonly CookieAuthService _cookieAuthService;
        private readonly GeminiAIService _geminiService;

        public MyTicketsModel(ITicketRepository ticketRepository, CookieAuthService cookieAuthService, GeminiAIService geminiService)
        {
            _ticketRepository = ticketRepository;
            _cookieAuthService = cookieAuthService;
            _geminiService = geminiService;
        }

        public List<TicketDto> Tickets { get; set; } = new();
		public int PageNumber { get; set; } = 1;
		public int TotalPages { get; set; }
		public const int PageSize = 10;

		[BindProperty(SupportsGet = true)]
		public int pageNumber { get; set; } = 1;

		public async Task<IActionResult> OnGetAsync()
        {
            var userId = _cookieAuthService.GetCurrentUserId();
            if (userId == null)
                return RedirectToPage("/Login");

            var tickets = await _ticketRepository.GetAllByUserIdAsync(userId.Value);

			PageNumber = pageNumber;
			TotalPages = (int)Math.Ceiling(tickets.Count / (double)PageSize);

			var paginatedTickets = tickets
		        .Skip((PageNumber - 1) * PageSize)
		        .Take(PageSize)
		        .ToList();

			Tickets = paginatedTickets.Select(t => new TicketDto
			{
				Id = t.Id,
				Title = t.Title,
				Description = t.Description,
				IsResolved = t.IsResolved,
				CreatedAt = t.CreatedAt,
				AdminResponse = t.AdminResponse
			}).ToList();

			return Page();
        }

        public async Task<IActionResult> OnPostSolveAsync(int id)
        {
            var userId = _cookieAuthService.GetCurrentUserId();
            if (userId == null)
                return RedirectToPage("/Login");

            var ticket = await _ticketRepository.GetByIdAndUserIdAsync(id, userId.Value);
            if (ticket == null)
                return NotFound();

            ticket.IsResolved = true;
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            TempData["TicketSolvedMessage"] = "Ticket baþarýyla çözüldü.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnSolveAsync(int id)
        {
            var userId = _cookieAuthService.GetCurrentUserId();
            if (userId == null)
                return RedirectToPage("/Login");

            var ticket = await _ticketRepository.GetByIdAndUserIdAsync(id, userId.Value);
            if (ticket == null)
                return NotFound();

            ticket.IsResolved = false;
            ticket.AdminResponse = null;
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            TempData["TicketNotSolvedMessage"] = "Ticket çözülemedi.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAskAiAsync(int id)
        {
            var userId = _cookieAuthService.GetCurrentUserId();
            if (userId == null) return RedirectToPage("/Login");

            var ticket = await _ticketRepository.GetByIdAndUserIdAsync(id, userId.Value);
            if (ticket == null) return NotFound();

            var response = await _geminiService.GetAIResponseAsync(ticket.Description);
            ticket.AdminResponse = "[AI] " + response;

            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            TempData["AIMessage"] = "AI çözüm önerisi eklendi.";
            return RedirectToPage();
        }
    }
}
