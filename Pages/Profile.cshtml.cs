using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystem.Application.Interfaces;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Dtos;
using SupportSystemApp.Application.Services;
using System.Security.Claims;

namespace SupportSystemApp.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly CookieAuthService _cookieAuthService;
        private readonly PasswordHasherService _passwordHasherService;

		public ProfileModel(IUserRepository userRepository, CookieAuthService cookieAuthService, PasswordHasherService passwordHasherService)
		{
			_cookieAuthService = cookieAuthService;
			_passwordHasherService = passwordHasherService;
			_userRepository = userRepository;
		}

		public ProfileDto profileDto { get; set; } = new();
        [BindProperty]
        public ChangePasswordDto passwordModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = _cookieAuthService.GetCurrentUser();
            if (user == null)
            {
                RedirectToPage("/Login");
            }

            profileDto = new ProfileDto
            {
                Username = user!.Identity!.Name!,
                Email = user.FindFirst(ClaimTypes.Email)?.Value!
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _cookieAuthService.GetCurrentUserId();
            if (userId == null)
                return RedirectToPage("/Login");

			var user = await _userRepository.GetByIdAsync((Guid)userId);
			if (user == null)
                return RedirectToPage("/Login");

            if (!_passwordHasherService.VerifyPassword(passwordModel.CurrentPassword, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Mevcut þifre hatalý.");
                return Page();
            }

            user.PasswordHash = _passwordHasherService.HashPassword(passwordModel.NewPassword);
            await _userRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Þifreniz baþarýyla güncellendi.";
            return RedirectToPage("/Profile");
        }
    }
}
