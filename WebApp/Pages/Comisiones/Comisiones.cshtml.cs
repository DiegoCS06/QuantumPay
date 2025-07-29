using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Comisiones
{
    [Authorize(Roles = "Admin")]
    public class ComisionesModel : PageModel
    {
        public void OnGet() { }
    }
}