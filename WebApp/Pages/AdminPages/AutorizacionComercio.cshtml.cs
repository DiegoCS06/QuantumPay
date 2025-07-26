using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.AdminPages
{
    [Authorize(Roles = "Admin")]
    public class AutorizacionComercioModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
