using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ClientesPages
{
    [Authorize(Roles = "Cliente")]
    public class BancosModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
