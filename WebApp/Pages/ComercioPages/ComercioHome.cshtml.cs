using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Collections.Generic;
using WebAPI.Services;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace WebApp.Pages.ComercioPages
{
    [Authorize(Roles = "CuentaComercio")]
    public class ComercioHomeModel : PageModel
    {
        private readonly ComercioService _comercioService;
        private readonly TransaccionService _transaccionService;
        private readonly CuentaComercioService _cuentaComercioService;

        public Comercio? Comercio { get; set; }
        public List<Transaccion> Transacciones { get; set; } = new();
        public ComercioInputModel NuevoComercio { get; set; } = new();

        [BindProperty]
        public NuevoAdminInputModel NuevoAdmin { get; set; } = new();

        public ComercioHomeModel(
            ComercioService comercioService,
            TransaccionService transaccionService,
            CuentaComercioService cuentaComercioService)
        {
            _comercioService = comercioService;
            _transaccionService = transaccionService;
            _cuentaComercioService = cuentaComercioService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var cuentaIdStr = User.FindFirstValue("UserId");
            if (!int.TryParse(cuentaIdStr, out int cuentaId))
            {
                ViewData["ComercioError"] = "No se pudo identificar la cuenta.";
                return Page();
            }

            Comercio = await _comercioService.GetByCuentaIdAsync(cuentaId);

            if (Comercio != null)
            {
                Transacciones = await _transaccionService.GetTransaccionesPorComercioAsync(Comercio.Id);
            }
            else
            {
                ViewData["ComercioError"] = "No se encontró un comercio asociado a esta cuenta.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cuentaIdStr = User.FindFirstValue("CuentaId");
            if (!int.TryParse(cuentaIdStr, out int cuentaId))
            {
                ViewData["ComercioError"] = "No se pudo identificar la cuenta.";
                await OnGetAsync();
                return Page();
            }

            // ⬇️ Binding manual
            if (!await TryUpdateModelAsync(NuevoComercio, "NuevoComercio"))
            {
                ViewData["ComercioError"] = "Por favor complete todos los campos correctamente.";
                await OnGetAsync();
                return Page();
            }

            if (!TryValidateModel(NuevoComercio))
            {
                ViewData["ComercioError"] = "Por favor complete todos los campos correctamente.";
                await OnGetAsync();
                return Page();
            }

            var nuevoComercio = new Comercio
            {
                Nombre = NuevoComercio.Nombre,
                IdCuenta = cuentaId,
                estadoSolicitud = "pendiente"
            };

            try
            {
                await _comercioService.CreateComercioAsync(nuevoComercio);
                ViewData["ComercioCreado"] = "Comercio creado correctamente. Espere la aprobación del administrador.";
                NuevoComercio = new ComercioInputModel();
            }
            catch (Exception ex)
            {
                ViewData["ComercioError"] = "Error al crear el comercio: " + ex.Message;
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAgregarAdminAsync()
        {

            foreach (var key in ModelState.Keys.Where(k => k.StartsWith("NuevoComercio")).ToList())
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                ViewData["AdminError"] = "Por favor complete todos los campos correctamente.";
                await OnGetAsync();
                return Page();
            }

            try
            {
                var cuentaIdStr = User.FindFirstValue("UserId");
                if (!int.TryParse(cuentaIdStr, out int cuentaId))
                {
                    ViewData["AdminError"] = "No se pudo identificar la cuenta.";
                    await OnGetAsync();
                    return Page();
                }

                var comercioActual = await _comercioService.GetByCuentaIdAsync(cuentaId);
                if (comercioActual == null)
                {
                    ViewData["AdminError"] = "No se encontró el comercio asociado a la cuenta.";
                    await OnGetAsync();
                    return Page();
                }

                await _cuentaComercioService.CreateCuentaComercioAsync(new CuentaComercio
                {
                    NombreUsuario = NuevoAdmin.NombreUsuario,
                    CorreoElectronico = NuevoAdmin.CorreoElectronico,
                    Telefono = int.TryParse(NuevoAdmin.Telefono, out int telefono) ? telefono : 0,
                    CedulaJuridica = NuevoAdmin.CedulaJuridica,
                    Direccion = NuevoAdmin.Direccion,
                    Contrasena = NuevoAdmin.Contrasena,
                    IdComercio = comercioActual.Id
                });

                ViewData["AdminCreado"] = "Administrador agregado correctamente.";
                ModelState.Clear();
                NuevoAdmin = new NuevoAdminInputModel();
            }
            catch (Exception ex)
            {
                ViewData["AdminError"] = "Error al agregar administrador: " + ex.Message;
            }

            await OnGetAsync();
            return Page();
        }
    }

    public class ComercioInputModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; }
    }

    public class NuevoAdminInputModel
    {
        [Required]
        public string NombreUsuario { get; set; }
        [Required, EmailAddress]
        public string CorreoElectronico { get; set; }
        [Required]
        public string CedulaJuridica { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string Contrasena { get; set; }
    }
}
