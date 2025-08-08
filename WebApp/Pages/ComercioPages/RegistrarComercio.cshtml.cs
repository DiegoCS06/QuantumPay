using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.Services;

namespace WebApp.Pages.ComercioPages
{
    public class RegistrarComercioModel : PageModel
    {
        private readonly ComercioService _comercioService;
        private readonly CuentaComercioService _cuentaComercioService;

        [BindProperty]
        public Comercio Comercio { get; set; }

        public RegistrarComercioModel(ComercioService comercioService, CuentaComercioService cuentaComercioService)
        {
            _comercioService = comercioService;
            _cuentaComercioService = cuentaComercioService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cuentaId = int.Parse(User.FindFirst("CuentaId")?.Value);

            Comercio.IdCuenta = cuentaId;
            await _comercioService.CreateComercioAsync(Comercio);

            // Obtener el comercio recién creado
            var comercioCreado = await _comercioService.GetByCuentaIdAsync(cuentaId);

            // Asociar el comercio a la cuenta
            await _cuentaComercioService.AsociarComercioAsync(cuentaId, comercioCreado.Id);

            return RedirectToPage("/ComercioPages/ComercioHome");
        }
    }
}