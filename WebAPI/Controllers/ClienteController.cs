// WebAPI/Controllers/ClienteController.cs
using Amazon;
using BaseManager;
using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Cliente")]
    public class ClienteController : ControllerBase
    {
        // ====================================
        // Acciones públicas (no requieren cookie)
        // ====================================

        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<ActionResult<Cliente>> Create(
            [FromBody] Cliente cliente,
            [FromQuery] string emailCode,
            [FromQuery] string smsCode)
        {
            try
            {
                Console.WriteLine($"[CREATE] Registro de cliente: correo={cliente.correo}");

                // 1) Verificar código OTP por email
                var emailVerifier = new EmailVerificationManager();
                if (!emailVerifier.VerifyCode(cliente.correo, emailCode))
                    return BadRequest("Código de verificación de email inválido.");

                // 2) Verificar código OTP por SMS
                if (string.IsNullOrWhiteSpace(smsCode))
                    return BadRequest("El código de verificación SMS es requerido.");

                var smsVerifier = new SmsVerificationManager();
                if (!smsVerifier.VerifyCode(cliente.telefono, smsCode))
                    return BadRequest("Código de verificación SMS inválido.");

                // 3) Configurar AWS para reconocimiento facial
                var awsKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")
                                ?? throw new Exception("AWS_ACCESS_KEY_ID no configurada.");
                var awsSecret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
                                ?? throw new Exception("AWS_SECRET_ACCESS_KEY no configurada.");
                var regionName = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";
                var region = RegionEndpoint.GetBySystemName(regionName);
                var faceVerifier = new FaceRecognitionManager(awsKey, awsSecret, region);

                // 4) Validar imágenes en base64
                if (string.IsNullOrWhiteSpace(cliente.fotoCedula) ||
                    string.IsNullOrWhiteSpace(cliente.fotoPerfil))
                {
                    return BadRequest("Las imágenes de cédula y perfil son requeridas.");
                }
                if (!IsBase64String(cliente.fotoCedula) ||
                    !IsBase64String(cliente.fotoPerfil))
                {
                    return BadRequest("Las imágenes deben estar en formato base64 válido.");
                }

                var cedulaBytes = Convert.FromBase64String(cliente.fotoCedula);
                var selfieBytes = Convert.FromBase64String(cliente.fotoPerfil);

                // 5) Comparar rostro con cédula
                if (!await faceVerifier.VerifyFaceAsync(selfieBytes, cedulaBytes))
                    return BadRequest("La selfie no coincide con la imagen de la cédula.");

                // 6) Hashear la contraseña y crear el cliente
                cliente.contrasena = BCrypt.Net.BCrypt.HashPassword(cliente.contrasena);
                var cm = new ClienteManager();
                await cm.Create(cliente);

                Console.WriteLine("[CREATE] Cliente creado exitosamente.");
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CREATE] Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("SendEmailVerification")]
        public ActionResult SendEmailVerification([FromQuery] string email)
        {
            try
            {
                var ev = new EmailVerificationManager();
                ev.SendVerificationCode(email);
                return Ok("Código de verificación enviado por email.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("SendSmsVerification")]
        public ActionResult SendSmsVerification([FromQuery] string telefono)
        {
            try
            {
                var sv = new SmsVerificationManager();
                sv.SendVerificationCode(telefono);
                return Ok("Código de verificación enviado por SMS.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("SendPasswordResetCode")]
        public ActionResult SendPasswordResetCode([FromBody] string email)
        {
            try
            {
                var user = new ClienteManager().RetrieveByEmail(email);
                if (user == null) return NotFound("Usuario no encontrado.");

                var ev = new EmailVerificationManager();
                ev.SendVerificationCode(email);
                return Ok("Código de recuperación enviado por email.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public ActionResult ResetPassword([FromBody] PasswordResetRequest request)
        {
            try
            {
                var ev = new EmailVerificationManager();
                if (!ev.VerifyCode(request.email, request.Code))
                    return BadRequest("Código de verificación inválido.");

                var cm = new ClienteManager();
                var user = cm.RetrieveByEmail(request.email);
                if (user == null) return NotFound("Usuario no encontrado.");

                user.contrasena = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                cm.Update(user);
                return Ok("Contraseña actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var cm = new ClienteManager();
                var user = cm.RetrieveByEmail(request.Email);
                if (user == null ||
                    !BCrypt.Net.BCrypt.Verify(request.Password, user.contrasena))
                {
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                // Construir claims y firmar cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name,           user.correo),
                    new Claim(ClaimTypes.Role,           "Cliente")
                };
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                    });

                return Ok(new { Message = "Login exitoso", UserId = user.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Sesión cerrada correctamente." });
        }

        // ====================================
        // Acciones protegidas (requieren cookie)
        // ====================================

        [HttpGet("RetrieveAll")]
        public ActionResult<List<Cliente>> RetrieveAll()
        {
            var cm = new ClienteManager();
            return Ok(cm.RetrieveAll());
        }

        [HttpGet("RetrieveById/{id}")]
        public ActionResult<Cliente> RetrieveById(int id)
        {
            var cm = new ClienteManager();
            var c = cm.RetrieveById(id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpGet("RetrieveByEmail")]
        public ActionResult<Cliente> RetrieveByEmail([FromQuery] string correo)
        {
            var cm = new ClienteManager();
            var c = cm.RetrieveByEmail(correo);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpGet("RetrieveByCedula")]
        public ActionResult<Cliente> RetrieveByCedula([FromQuery] string cedula)
        {
            var cm = new ClienteManager();
            var c = cm.RetrieveByCedula(cedula);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpGet("RetrieveByTelefono")]
        public ActionResult<Cliente> RetrieveByTelefono([FromQuery] string telefono)
        {
            var cm = new ClienteManager();
            var c = cm.RetrieveByTelefono(telefono);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpPut("Update")]
        public ActionResult<Cliente> Update([FromBody] Cliente cliente)
        {
            if (!string.IsNullOrWhiteSpace(cliente.contrasena))
                cliente.contrasena = BCrypt.Net.BCrypt.HashPassword(cliente.contrasena);

            var cm = new ClienteManager();
            var updated = cm.Update(cliente);
            return Ok(updated);
        }

        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var cm = new ClienteManager();
            cm.Delete(id);
            return Ok(new { Message = $"Usuario con ID {id} eliminado correctamente." });
        }

        // ==================
        // Métodos auxiliares
        // ==================

        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return false;
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}
