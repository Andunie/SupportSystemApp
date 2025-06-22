using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Application.Interfaces;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Dtos;
using SupportSystemApp.Application.Services;

namespace SupportSystemApp.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly CookieAuthService _cookieAuthService;

        public UsersModel(IUserRepository userRepository, CookieAuthService cookieAuthService)
        {
            _userRepository = userRepository;
            _cookieAuthService = cookieAuthService;   
        }

        public List<UsersDto> users { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public const int PageSize = 10;
        [BindProperty(SupportsGet = true)]
        public int pageNumber { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = _cookieAuthService.GetCurrentUser();
            if (user == null || user.FindFirst("IsAdmin")?.Value != "True")
            {
                return RedirectToPage("/Login");
            }

            PageNumber = pageNumber;
            var totalUserCount = await _userRepository.GetTotalUserCountAsync();
            TotalPages = (int)Math.Ceiling(totalUserCount / (double)PageSize);

            users = await _userRepository.GetPaginatedUsersAsync(PageNumber, PageSize);

            return Page();
        }


    }
}