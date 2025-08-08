using BaseManager;
using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Cliente,InstitucionBancaria")]
    [Route("api/[controller]")]
    [ApiController]
    public class InstitucionBancariaController : ControllerBase 
    {
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create([FromBody] InstitucionBancaria institucionBancaria)
        {
            try
            {
                institucionBancaria.contrasena = BCrypt.Net.BCrypt.HashPassword(institucionBancaria.contrasena);

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
        public ActionResult<IEnumerable<InstitucionBancaria>> RetrieveAll()
        {
            try
            {
                var tm = new InstitucionBancariaManager();
                
                var all = tm.RetrieveAll();
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
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
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
        public ActionResult RetrieveByCodigoIdentidad(string codigoIdentidad)
        {
            try
            {
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
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
        [Route("RetrieveByTelefono")]
        public ActionResult Retrieve(int telefono)
        {
            try
            {
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
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
                var user = HttpContext.User;
                var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
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
        public ActionResult Login([FromBody] DTOs.LoginRequest request)
        {
            try
            {
                var im = new InstitucionBancariaManager();
                var user = im.RetrieveByEmail(request.Email);
                if (user == null)
                    return Unauthorized("Usuario o contraseña incorrectos.");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.contrasena))
                    return Unauthorized("Usuario o contraseña incorrectos.");

                return Ok(new { Message = "Login exitoso", UserId = user.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("SendPasswordResetCode")]
        public ActionResult SendPasswordResetCode([FromBody] string email)
        {
            var user = new InstitucionBancariaManager().RetrieveByEmail(email);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            var emailVerifier = new EmailVerificationManager();
            emailVerifier.SendVerificationCode(email);
            return Ok("Código de recuperación enviado por email.");
        }

        [HttpPost]
        [Route("ResetPassword")]
        public ActionResult ResetPassword([FromBody] DTOs.PasswordResetRequest request)
        {
            var emailVerifier = new EmailVerificationManager();
            bool verified = emailVerifier.VerifyCode(request.email, request.Code);
            if (!verified)
                return BadRequest("Código de verificación inválido.");

            var im = new InstitucionBancariaManager();
            var user = im.RetrieveByEmail(request.email);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            user.contrasena = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            im.Update(user);
            return Ok("Contraseña actualizada correctamente.");
        }
    }
}
