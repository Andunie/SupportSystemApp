using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystemApp.Application.Services;

namespace SupportSystemApp.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly CookieAuthService _cookieAuthService;

        public LogoutModel(CookieAuthService cookieAuthService)
        {
            _cookieAuthService = cookieAuthService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _cookieAuthService.SignOutAsync();
            return RedirectToPage("/Login");
        }
    }
}
