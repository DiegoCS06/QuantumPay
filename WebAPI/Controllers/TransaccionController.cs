using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly TransaccionManager _manager = new();

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Crear([FromBody] Transaccion transaccion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 1. Get user info from JWT
                var userEmail = User.Identity.Name;
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                int userId = 0;
                int.TryParse(userIdClaim, out userId);

                // 2. Retrieve client name (NombreCliente) from DB
                var clienteManager = new ClienteManager();
                var cliente = clienteManager.RetrieveById(userId);
                transaccion.NombreCliente = cliente?.nombre ?? "";

                // 3. Retrieve institucion bancaria info and bank code
                var institucionBancariaManager = new InstitucionBancariaManager();
                var institucionBancaria = institucionBancariaManager.RetrieveById(transaccion.IdCuentaBancaria);
                transaccion.CodigoIdentidadInstitucionBancaria = institucionBancaria?.codigoIdentidad ?? "";

                await _manager.Create(transaccion);
                return Ok(new { message = "Transacción creada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult<IEnumerable<Transaccion>> RetrieveAll()
        {
            try
            {
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                var tm = new TransaccionManager();
                var lstResults = tm.RetrieveAll(int.Parse(userId), userRole);
                return Ok(lstResults);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveById")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                var tm = new TransaccionManager();
                var result = tm.OrdenarPorId(id);
                if (result == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByBanco")]
        public ActionResult RetrieveByBanco(int idCuentaBancaria)
        {
            try
            {
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var cm = new TransaccionManager();
                var result = cm.OrdenarPorBanco(idCuentaBancaria);
                if (result == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByComercio")]
        public ActionResult RetrieveByComercio(int idComercio)
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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByCliente")]
        public ActionResult RetrieveByCliente(int idCliente)
        {
            try
            {
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var cm = new TransaccionManager();
                var result = cm.OrdenarPorCliente(idCliente);
                if (result == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody] Transaccion transaccion)
        {
            try
            {
                _manager.Update(transaccion);
                return Ok(new { message = "Transacción actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var cm = new TransaccionManager();
                var existing = cm.OrdenarPorId(id);
                cm.Delete(id);
                return Ok(new { Message = $"Transaccion con ID {id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}