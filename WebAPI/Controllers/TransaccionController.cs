// WebAPI/Controllers/TransaccionController.cs
using BaseManager;
using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Cliente")]
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public TransaccionController(IEmailSender emailSender)
            => _emailSender = emailSender;

        [HttpPost("Create")]
        public async Task<ActionResult<Transaccion>> Create(
            [FromBody] Transaccion t,
            [FromQuery] string email)
        {
            try
            {
                // 1) Crear la transacción
                var mgr = new TransaccionManager();
                mgr.Create(t);

                // 2) Enviar correo si recibimos email
                if (!string.IsNullOrWhiteSpace(email))
                {
                    await _emailSender.SendEmailAsync(
                        toEmail: email,
                        subject: "Confirmación de compra",
                        message: $"Compra por {t.Monto:C} procesada."
                    );
                }

                return Ok(t);
            }
            catch (System.Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message
                );
            }
        }

        [HttpPut("Update/{id}")]
        public ActionResult<Transaccion> Update(
            int id,
            [FromBody] Transaccion t)
        {
            try
            {
                t.Id = id;
                var mgr = new TransaccionManager();
                var updated = mgr.Update(t);
                return Ok(updated);
            }
            catch (System.Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message
                );
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var cm = new TransaccionManager();
                var result = cm.OrdenarPorComercio(idComercio);
                if (result == null || !result.Any())
                {
                    return Ok(new List<Transaccion>());
                }
                return Ok(result); // <-- Devuelve la lista directamente
            }
            catch (System.Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message
                );
            }
        }

        [HttpGet("RetrieveAll")]
        public ActionResult<IEnumerable<Transaccion>> RetrieveAll()
        {
            try
            {
                var user = HttpContext.User;

                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (userId == null || userRole == null)
                    return Unauthorized("No se pudo verificar la identidad del usuario.");

                int Id = int.Parse(userId);

                var tm = new TransaccionManager();

                if (userRole == "Admin")
                {
                    // Admin ve todas las transacciones
                    var all = tm.RetrieveAll();
                    return Ok(all);
                }
                else
                {
                    // Otros roles ven solo sus transacciones
                    var userTransactions = tm.RetrieveByCliente(Id);
                    return Ok(userTransactions);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("RetrieveByBanco")]
        public ActionResult<IEnumerable<Transaccion>> RetrieveByBanco(
            [FromQuery] int cuentaId)
            => Ok(new TransaccionManager().RetrieveByCuenta(cuentaId));

        [HttpGet("RetrieveByComercio")]
        public ActionResult<IEnumerable<Transaccion>> RetrieveByComercio(
            [FromQuery] int idComercio)
            => Ok(new TransaccionManager().RetrieveByComercio(idComercio));

        [HttpGet("RetrieveByCliente")]
        public ActionResult<IEnumerable<Transaccion>> RetrieveByCliente(
            [FromQuery] int clienteId)
            => Ok(new TransaccionManager().RetrieveByCliente(clienteId));
    }
}

