using BaseManager;
using BCrypt.Net;
using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Cliente,CuentaComercio")]
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaComercioController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create([FromBody] CuentaComercio cuentaComercio)
        {
            try
            {
                cuentaComercio.Contrasena = BCrypt.Net.BCrypt.HashPassword(cuentaComercio.Contrasena);
                var cm = new CuentaComercioManager();
                await cm.Create(cuentaComercio);
                return Ok(cuentaComercio);
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
        [Route("RetrieveByEmail")]
        public ActionResult RetrieveByEmail(string CorreoElectronico)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var result = cm.RetrieveByEmail(CorreoElectronico);
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
        [Route("RetrieveByUserName")]
        public ActionResult RetrieveByUserName(string NombreUsuario)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var result = cm.RetrieveByUserName(NombreUsuario);
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
        [Route("RetrieveByTelefono")]
        public ActionResult RetrieveByTelefono(int telefono)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var result = cm.RetrieveByTelefono(telefono);
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
        public ActionResult Update(CuentaComercio cuentaComercio)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cuentaComercio.Contrasena))
                {
                    cuentaComercio.Contrasena = BCrypt.Net.BCrypt.HashPassword(cuentaComercio.Contrasena);
                }

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
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var cm = new CuentaComercioManager();
                var existing = cm.RetrieveById(id);
                cm.Delete(id);
                return Ok(new { Message = $"CuentaComercio con ID {id} eliminado correctamente." });
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
                var cm = new CuentaComercioManager();
                var user = cm.RetrieveByEmail(request.Email);
                if (user == null)
                    return Unauthorized("Usuario o contraseña incorrectos.");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Contrasena))
                    return Unauthorized("Usuario o contraseña incorrectos.");

                return Ok(new { Message = "Login exitoso", UserId = user.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Sesión cerrada correctamente." });
        }

        [HttpPost]
        [Route("SendPasswordResetCode")]
        public ActionResult SendPasswordResetCode([FromBody] string email)
        {
            var user = new CuentaComercioManager().RetrieveByEmail(email);
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

            var cm = new CuentaComercioManager();
            var user = cm.RetrieveByEmail(request.email);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            user.Contrasena = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            cm.Update(user);
            return Ok("Contraseña actualizada correctamente.");
        }
    }
}