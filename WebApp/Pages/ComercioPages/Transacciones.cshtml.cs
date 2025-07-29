using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ComercioPages
{
    [Authorize(Roles = "CuentaComercio")]
    public class TransaccionesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
