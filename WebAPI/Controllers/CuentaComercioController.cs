using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaComercioController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(CuentaComercio cuentacomercio)
        {
            try
            {
                var cm = new CuentaComercioManager();
                await cm.Create(cuentacomercio);
                return Ok(cuentacomercio);
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
                var cm = new CuentaComercioManager();
                var lstResults = cm.RetrieveAll();
                return Ok(lstResults);
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
                var cm = new CuentaComercioManager();
                var result = cm.RetrieveById(Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByEmail")]
        public ActionResult RetrieveByEmail(string CorreoElectronico)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var result = cm.RetrieveByEmail(CorreoElectronico);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("RetrieveByUserName")]
        public ActionResult RetrieveByUserName(string NombreUsuario)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var result = cm.RetrieveByUserName(NombreUsuario);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("RetrieveByTelefono")]
        public ActionResult RetrieveByTelefono(int telefono)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var result = cm.RetrieveByTelefono(telefono);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        [Route("Update")]
        public ActionResult Update(CuentaComercio cuentaComercio)
        {
            try
            {
                var cm = new CuentaComercioManager();
                cm.Update(cuentaComercio);
                return Ok(cuentaComercio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public ActionResult Delete(Cliente cliente)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var existing = cm.RetrieveById(cliente.Id);
                cm.Delete(cliente.Id);
                return Ok(new { Message = $"Usuario con ID {cliente.Id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
    }
}
