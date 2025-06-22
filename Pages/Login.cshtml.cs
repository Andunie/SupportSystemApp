using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystem.Application.Services;
using SupportSystemApp.Application.Services;

namespace SupportSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthenticationService _authenticationService;
        private readonly CookieAuthService _cookieAuthService;

        public LoginModel(AuthenticationService authService, CookieAuthService cookieAuthService)
        {
            _authenticationService = authService;
            _cookieAuthService = cookieAuthService;
        }

        [BindProperty] public string UsernameOrEmail { get; set; } = string.Empty;
        [BindProperty] public string Password { get; set; } = string.Empty;
        public string? Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _authenticationService.AuthenticateAsync(UsernameOrEmail, Password);
            if (user == null) 
            {
                TempData["LoginFailed"] = "Invalid credentials.";
                return Page();
            }
                
            await _cookieAuthService.SignInAsync(user);

            if (user.IsAdmin)
            {
                return RedirectToPage("/Admin/AllTickets");
            }
            else
                return RedirectToPage("Home");
        }
    }
}
