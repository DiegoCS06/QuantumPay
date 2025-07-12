using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitucionBancariaController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(InstitucionBancaria institucionBancaria)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                await im.Create(institucionBancaria);
                return Ok(institucionBancaria);
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
                var im = new InstitucionBancariaManager();
                var lstResults = im.RetrieveAll();
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
                var im = new InstitucionBancariaManager();
                var result = im.RetrieveById(Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByIBAN")]
        public ActionResult RetrieveByIBAN(int codigoIBAN)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var result = im.RetrieveByIBAN(codigoIBAN);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByTelefono")]
        public ActionResult Retrieve(int telefono)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var result = im.RetrieveByTelefono(telefono);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("RetrieveByEmail")]
        public ActionResult RetrieveByEmail(string correoElectronico)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var result = im.RetrieveByEmail(correoElectronico);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpPut]
        [Route("Update")]
        public ActionResult Update(InstitucionBancaria institucionBancaria)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                im.Update(institucionBancaria);
                return Ok();
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
                var im = new InstitucionBancariaManager();
                var existing = im.RetrieveById(cliente.Id);
                im.Delete(cliente.Id);
                return Ok(new { Message = $"Usuario con ID {cliente.Id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
    }
}
