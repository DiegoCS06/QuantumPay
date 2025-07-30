using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ClientesPages
{
    [Authorize(Roles = "Cliente")]
    public class PromocionesActivasModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
