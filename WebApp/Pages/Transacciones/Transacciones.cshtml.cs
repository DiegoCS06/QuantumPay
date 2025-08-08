using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Transacciones
{
    public class TransaccionesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int ClienteId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        public void OnGet()
        {

        }
    }
}
