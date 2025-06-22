using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystem.Application.Interfaces;
using SupportSystemApp.Application.Services;

namespace SupportSystemApp.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;

		public ForgetPasswordModel(IUserRepository userRepository, EmailService emailService)
		{
			_userRepository = userRepository;
			_emailService = emailService;
		}

		[BindProperty]
        public string Email { get; set; }
		public string Message { get; set; }

		public async Task<IActionResult> OnPostAsync()
        {
			var user = await _userRepository.GetByUsernameOrEmailAsync(Email);

			if (user == null)
			{
				ModelState.AddModelError("", "There's no user with this email.");
				return Page();
			}

			var resetToken = Guid.NewGuid().ToString();

			var resetLink = Url.Page(
				"/ResetPassword",
				pageHandler: null,
				values: new { email = Email, token = resetToken },
				protocol: Request.Scheme);

			await _emailService.SendEmailForResetPassword(Email, resetLink);

			Message = "Your reset password link sent to your email address.";
			return Page();
		}
    }
}
