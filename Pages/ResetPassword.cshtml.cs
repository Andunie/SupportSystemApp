using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportSystem.Application.Interfaces;
using SupportSystemApp.Application.Services;
using System.ComponentModel.DataAnnotations;

namespace SupportSystemApp.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasherService _passwordHasherService;

        public ResetPasswordModel(IUserRepository userRepository, PasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
        }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords does not match!")]
        public string ConfirmPassword { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Does not find user");
                return Page();
            }

            // Bu örnekte Token kontrolü yapýlmýyor. Gerçek projede veritabanýna yazýp kontrol etmelisin!
            var hashedPassword = _passwordHasherService.HashPassword(NewPassword);
            user.PasswordHash = hashedPassword;

            await _userRepository.SaveChangesAsync();

            Message = "Your password has been successfully reset.";
            return Page();
        }
    }
}
