using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystem.Domain.Entities;
using SupportSystemApp.Application.Interfaces;
using SupportSystemApp.Application.Services;
using SupportSystemApp.Domain.Entities;

namespace SupportSystemApp.Pages
{
    public class CreateTicketModel : PageModel
    {
		private readonly ITicketRepository _ticketRepository;
		private readonly INotificationRepository _notificationRepository;
        private readonly CookieAuthService _cookieAuthService;
		private readonly NotificationService _notificationService;
		private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GeminiAIService _geminiAIService;

        public CreateTicketModel(ITicketRepository ticketRepository, INotificationRepository notificationRepository, CookieAuthService cookieAuthService,
            NotificationService notificationService, IWebHostEnvironment webHostEnvironment, GeminiAIService geminiAIService)
        {
            _ticketRepository = ticketRepository;
            _notificationRepository = notificationRepository;
            _cookieAuthService = cookieAuthService;
            _notificationService = notificationService;
            _webHostEnvironment = webHostEnvironment;
            _geminiAIService = geminiAIService;
        }

        [BindProperty]
        public string Title { get; set; } = string.Empty;
		[BindProperty]
		public string Description { get; set; } = string.Empty;
        [BindProperty]
        public IFormFile? UploadImage { get; set; }

        public async Task<IActionResult> OnPostAsync()
		{
			var user = _cookieAuthService.GetCurrentUser();
			if (user == null)
			{
				return RedirectToPage("/Login");
			}

			var userId = _cookieAuthService.GetCurrentUserId();
			if (userId == null)
				return RedirectToPage("/Login");

			string? imagePath = null;
            if (UploadImage != null && UploadImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "tickets");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadImage.CopyToAsync(fileStream);
                }

                imagePath = $"/uploads/tickets/{uniqueFileName}";
            }

            string priorty = await _geminiAIService.GetPriortyAsync(Title, Description);
            priorty = priorty.Trim();

            var ticket = new Ticket
			{
				Title = Title,
				Description = Description,
				CreatedAt = DateTime.UtcNow,
				CreatedByUserId = (Guid)_cookieAuthService.GetCurrentUserId()!,
				IsResolved = false,
                ImagePath = imagePath,
                Priorty = priorty
			};

			await _ticketRepository.AddAsync(ticket);
			await _ticketRepository.SaveChangesAsync();

			TempData["TicketCreatedMessage"] = "Ticket Created Successfully!";
			await _notificationService.NotifyAllAdminAsync((Guid)userId, "A new ticket created");

			return RedirectToPage("/CreateTicket");
		}
	}
}
