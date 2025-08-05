using CoreApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ClientesPages
{
    [Authorize(Roles = "Cliente")]
    public class NuevaTransaccionModel : PageModel
    {
        public int ClienteId { get; private set; }

        public string Email { get; private set; } = string.Empty;

        public IActionResult OnGet()
        {
            Email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(Email))
                return RedirectToPage("/Account/Login");

            var cliente = new ClienteManager().RetrieveByEmail(Email);
            if (cliente == null)
                return RedirectToPage("/Error");

            ClienteId = cliente.IdCliente;
            return Page();
        }
    }
}
