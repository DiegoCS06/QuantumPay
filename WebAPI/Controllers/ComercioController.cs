using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Cliente")]
    [Route("api/[controller]")]
    [ApiController]
    public class ComercioController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(Comercio comercio)
        {
            try
            {
                var cm = new ComercioManager();
                await cm.Create(comercio);
                return Ok(comercio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {                
                var cm = new ComercioManager();

                var all = cm.RetrieveAll();
                return Ok(all);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveById")]
        public ActionResult RetrieveById(int Id)
        {
            try
            {
                var cm = new ComercioManager();
                var result = cm.RetrieveById(Id);

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
        [Route("RetrieveByComercioName")]
        public ActionResult RetrieveByComercioName(string nombre)
        {
            try
            {
                var cm = new ComercioManager();
                var result = cm.RetrieveByComercioName(nombre);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        [Route("Update")]
        public ActionResult Update(Comercio comercio)
        {
            try
            {
                var cm = new ComercioManager();
                var updated = cm.Update(comercio);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var cm = new ComercioManager();
                var existing = cm.RetrieveById(id);
                cm.Delete(id);
                return Ok(new { Message = $"Comercio con ID {id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByCuentaId/{cuentaId}")]
        public ActionResult<Comercio> GetByCuentaId(int cuentaId)
        {
            var ccm = new CuentaComercioManager();
            var cuenta = ccm.RetrieveById(cuentaId);
            if (cuenta == null || !cuenta.IdComercio.HasValue)
                return NotFound();

            var cm = new ComercioManager();

            var comercio = cm.RetrieveById(cuenta.IdComercio.Value);
            if (comercio == null)
                return NotFound();

            return Ok(comercio);
        }
    }
}

