using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace WebApp.Pages.Clientes
{
    [Authorize(Roles = "Cliente")]
    public class ClienteHomeModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int ClienteId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClienteEmail { get; set; }

        public void OnGet()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out var id))
            {
                ClienteId = id;
            }

            ClienteEmail = User.Identity?.Name ?? string.Empty;
        }
    }
}
