using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Services;

namespace SupportSystemApp.Pages.Admin
{
    public class AllTicketsModel : PageModel
    {
        private readonly AppDbContext _context; // Kaldýrýlacak
        private readonly CookieAuthService _cookieAuthService;

        public AllTicketsModel(AppDbContext context, CookieAuthService cookieAuthService)
        {
            _context = context;
            _cookieAuthService = cookieAuthService;
        }

        public List<Ticket> tickets { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? CurrentPriority { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public async Task<IActionResult> OnGetAsync(string? priority, int pageNumber = 1)
        {
            var user = _cookieAuthService.GetCurrentUser();
            if (user == null || user.FindFirst("IsAdmin")?.Value != "True")
            {
                return RedirectToPage("/Login");
            }

            CurrentPriority = priority;
            PageNumber = pageNumber;

            var query = _context.Tickets.AsQueryable();

            if (!string.IsNullOrEmpty(priority))
            {
                query = query.Where(t => t.Priorty == priority);
            }

            int totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            tickets = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostChangePriorityAsync(int ticketId, string newPriority)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return NotFound();

            ticket.Priorty = newPriority;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { priority = CurrentPriority }); // filtreyi koru
        }
    }
}
