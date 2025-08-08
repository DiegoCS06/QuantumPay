using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{

    [Authorize(Roles = "Admin,Cliente")]
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaClienteController : ControllerBase
    {
        [HttpGet("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var user = HttpContext.User;

                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (userId == null || userRole == null)
                    return Unauthorized("No se pudo verificar la identidad del usuario.");

                int Id = int.Parse(userId);

                var m = new CuentaClienteManager();
                var list = m.Listar(int.Parse(userId));

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Create")]
        public ActionResult Create([FromBody] ClienteCuenta dto)
        {
            try
            {
                var m = new CuentaClienteManager();
                m.Crear(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Update")]
        public ActionResult Update([FromBody] ClienteCuenta dto)
        {
            try
            {
                var m = new CuentaClienteManager();
                m.Actualizar(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var m = new CuentaClienteManager();
                m.Eliminar(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
