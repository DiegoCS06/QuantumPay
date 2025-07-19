using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Transacciones
{
    public class BancoModel : PageModel
    {
        // Esto hace que el par�metro de ruta "idBanco" se pupule aqu�
        [BindProperty(SupportsGet = true)]
        public int IdBanco { get; set; }

        public void OnGet()
        {
            // No necesitas asignar nada, IdBanco ya est� lleno
        }
    }
}
