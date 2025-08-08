using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Clientes
{
    [Authorize(Roles = "Cliente")]
    public class ClienteHomeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
