using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SupportSystemApp.Pages
{
    [Authorize]
    public class HomeModel : PageModel
    {
        public void OnGet()
        {
           
        }
    }
}
