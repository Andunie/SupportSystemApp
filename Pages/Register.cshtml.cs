using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystem.Application.Services;

namespace SupportSystem.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AuthenticationService _authenticationService;

        public RegisterModel(AuthenticationService authService)
        {
            _authenticationService = authService;
        }

        [BindProperty] public string Username { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Password { get; set; } = string.Empty;
        public string? Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _authenticationService.RegisterAsync(Username, Email, Password);
            Message = user == null ? "Registration failed." : "User registered successfully!";
            return Page();
        }
    }
}
