using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(Cliente cliente)
        {
            try
            {
                var cm = new ClienteManager();
                await cm.Create(cliente);
                return Ok(cliente);
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
                var cm = new ClienteManager();
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
                var cm = new ClienteManager();
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
        public ActionResult RetrieveByEmail(string correo)
        {
            try
            {
                var cm = new ClienteManager();
                var result = cm.RetrieveByEmail(correo);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("RetrieveByCedula")]
        public ActionResult Retrieve(string cedula)
        {
            try
            {
                var cm = new ClienteManager();
                var result = cm.RetrieveByCedula(cedula);

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
                var cm = new ClienteManager();
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
        public ActionResult Update(Cliente cliente)
        {
            try
            {
                var cm = new ClienteManager();
                var updatedCliente = cm.Update(cliente);
                return Ok(updatedCliente);
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
                var cm = new ClienteManager();
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
