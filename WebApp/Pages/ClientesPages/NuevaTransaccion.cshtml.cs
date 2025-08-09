using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace WebApp.Pages.ClientesPages
{
    [Authorize(Roles = "Cliente")]
    public class NuevaTransaccionModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClienteManager _clienteManager;

        public NuevaTransaccionModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _clienteManager = new ClienteManager();
        }

        public string NombreCliente { get; set; }

        public async Task OnGetAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = 0;
            int.TryParse(userIdClaim, out userId);
            var cliente = _clienteManager.RetrieveById(userId);
            NombreCliente = cliente?.nombre;
        }
    }
}
