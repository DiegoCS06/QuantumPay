using CoreApp;
using DTOs;
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
        [Route("RetrieveByCodigoIdentidad")]
        public ActionResult RetrieveByCodigoIdentidad(int codigoIdentidad)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var result = im.RetrieveByCodigoIdentidad(codigoIdentidad);

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
        [Route("RetrieveByIBAN")]
        public ActionResult RetrieveByIBAN(int codigoIBAN)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var result = im.RetrieveByIBAN(codigoIBAN);

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
        [Route("RetrieveByTelefono")]
        public ActionResult Retrieve(int telefono)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var result = im.RetrieveByTelefono(telefono);

                if (result == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { result });
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

                if (result == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { result });
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
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var existing = im.RetrieveById(id);
                im.Delete(id);
                return Ok(new { Message = $"InstitucionBancaria con ID {id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] DTOs.LoginInstiBancariaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.CedulaJuridica))
                    return Unauthorized("Usuario o cédula jurídica incorrectos.");

                var manager = new InstitucionBancariaManager();
                var institucion = manager.RetrieveByEmail(request.Email.Trim());

                if (institucion == null)
                    return Unauthorized("Usuario o cédula jurídica incorrectos.");

                // Normaliza y compara la cédula jurídica
                var cedulaInput = request.CedulaJuridica.Trim();
                var cedulaDb = institucion.cedulaJuridica?.Trim();

                if (!string.Equals(cedulaDb, cedulaInput, StringComparison.OrdinalIgnoreCase))
                    return Unauthorized("Usuario o cédula jurídica incorrectos.");

                if (!string.Equals(institucion.estadoSolicitud, "aprobada", StringComparison.OrdinalIgnoreCase))
                    return Unauthorized("Error al iniciar sesion, su institucion no ha sido aprobada.");

                return Ok(new { Message = "Login exitoso", UserId = institucion.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
