using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace WebApp.Pages.ClientesPages
{
    [Authorize(Roles = "Cliente")]
    public class TransaccionesClienteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int ClienteId { get; set; }

        public void OnGet()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out var id))
            {
                ClienteId = id;
            }
        }
    }
}
