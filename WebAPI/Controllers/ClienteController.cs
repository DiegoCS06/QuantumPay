using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BaseManager;
using BaseManager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly CloudinaryManager _cloudinaryManager = new CloudinaryManager();// Instancia del administrador de Cloudinary

        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(Cliente cliente, [FromQuery] string emailCode, [FromQuery] string smsCode)
        {
            try
            {
                Console.WriteLine($"[CREATE] Registro de cliente: correo={cliente.correo}, contrasena={cliente.contrasena}");
                //VERIFICADORES

                var emailVerifier = new EmailVerificationManager(); //instancia del verificador de email
                var smsVerifier = new SmsVerificationManager(); // instancia del verificador de SMS

                // Verificar código OTP email 1
                bool emailVerified = emailVerifier.VerifyCode(cliente.correo, emailCode);
                if (!emailVerified)
                    return BadRequest("Código de verificación de email inválido.");

                // Verificar código OTP de SMS
                if (string.IsNullOrWhiteSpace(smsCode))
                {
                    Console.WriteLine("[CREATE] smsCode vacío o nulo.");
                    return BadRequest("El código de verificación SMS es requerido.");
                }
                bool smsVerified = smsVerifier.VerifyCode(cliente.telefono, smsCode);
                if (!smsVerified)
                    return BadRequest("Código de verificación SMS inválido.");

                // Leer variables de entorno dentro de sistema (HAY QUE CONFIGURARLAS EN EL SERVIDOR)
                var awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")
                    ?? throw new Exception("AWS_ACCESS_KEY_ID no configurada.");
                var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")
                    ?? throw new Exception("AWS_SECRET_ACCESS_KEY no configurada.");
                var awsRegionName = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";
                var region = Amazon.RegionEndpoint.GetBySystemName(awsRegionName);

                var faceVerifier = new FaceRecognitionManager(awsAccessKeyId, awsSecretKey, region);

                if (cliente == null)
                {
                    Console.WriteLine("[CREATE] Cliente is null.");
                    return BadRequest("Cliente data is missing.");
                }
                if (string.IsNullOrWhiteSpace(cliente.fotoCedula) || string.IsNullOrWhiteSpace(cliente.fotoPerfil))
                {
                    Console.WriteLine("[CREATE] fotoCedula or fotoPerfil is null or empty.");
                    return BadRequest("Las imágenes son requeridas.");
                }
                if (!IsBase64String(cliente.fotoCedula) || !IsBase64String(cliente.fotoPerfil))
                {
                    Console.WriteLine("[CREATE] Invalid base64 for images.");
                    return BadRequest("Las imágenes deben estar en formato base64 válido.");
                }

                // Decodificar las imágenes de base64 a bytes
                byte[] cedulaBytes = Convert.FromBase64String(cliente.fotoCedula);
                byte[] selfieBytes = Convert.FromBase64String(cliente.fotoPerfil);

                try
                {
                    string folder= "clientes";
                    string urlFotoCedula = _cloudinaryManager.UploadBase64Image(cliente.fotoCedula, folder, "cedula_" + Guid.NewGuid().ToString());
                    string urlFotoPerfil = _cloudinaryManager.UploadBase64Image(cliente.fotoPerfil, folder, "perfil_" + Guid.NewGuid().ToString());
                    // Actualizar las propiedades con las URLs
                    cliente.fotoCedula = urlFotoCedula;
                    cliente.fotoPerfil = urlFotoPerfil;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CREATE] Error uploading images to Cloudinary: {ex.Message}");
                    return StatusCode(500, "Error al subir las imágenes." + ex.Message);
                }

                // Verificar selfie vs cédula
                bool faceMatch = await faceVerifier.VerifyFaceAsync(selfieBytes, cedulaBytes);
                if (!faceMatch)
                    return BadRequest("La selfie no coincide con la imagen de la cédula.");

                //SI PASA LAS VERIFICACIONES, CREAR EL CLIENTE

                cliente.contrasena = BCrypt.Net.BCrypt.HashPassword(cliente.contrasena);

                var cm = new ClienteManager();
                await cm.Create(cliente);
                Console.WriteLine("[CREATE] Cliente creado exitosamente.");
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CREATE] Excepción: {ex.Message}");
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
        public ActionResult RetrieveByEmail(string correo)
        {
            try
            {
                var cm = new ClienteManager();
                var result = cm.RetrieveByEmail(correo);

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
        [Route("RetrieveByCedula")]
        public ActionResult Retrieve(string cedula)
        {
            try
            {
                var cm = new ClienteManager();
                var result = cm.RetrieveByCedula(cedula);

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
        public ActionResult RetrieveByTelefono(string telefono)
        {
            try
            {
                var cm = new ClienteManager();
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
        public ActionResult Update(Cliente cliente)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cliente.contrasena))
                {
                    cliente.contrasena = BCrypt.Net.BCrypt.HashPassword(cliente.contrasena);
                }

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
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var cm = new ClienteManager();
                var existing = cm.RetrieveById(id);
                cm.Delete(id);
                return Ok(new { Message = $"Usuario con ID {id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("SendEmailVerification")]
        public ActionResult SendEmailVerification(string email)
        {
            try
            {
                var emailVerifier = new EmailVerificationManager();
                emailVerifier.SendVerificationCode(email);
                return Ok("Código de verificación enviado por email.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("SendSmsVerification")]
        public ActionResult SendSmsVerification(string telefono)
        {
            try
            {
                var smsVerifier = new SmsVerificationManager();
                smsVerifier.SendVerificationCode(telefono);
                return Ok("Código de verificación enviado por SMS.");
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
                Console.WriteLine($"[LOGIN] Email recibido: {request.Email}");
                Console.WriteLine($"[LOGIN] Password recibido: {request.Password}");

                var cm = new ClienteManager();
                var user = cm.RetrieveByEmail(request.Email);

                if (user == null)
                {
                    Console.WriteLine("[LOGIN] Usuario no encontrado en la base de datos.");
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                Console.WriteLine($"[LOGIN] Hash almacenado en BD: {user.contrasena}");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.contrasena))
                {
                    Console.WriteLine("[LOGIN] Contraseña incorrecta.");
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                Console.WriteLine("[LOGIN] Login exitoso.");
                return Ok(new { Message = "Login exitoso", UserId = user.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGIN] Excepción: {ex.Message}");
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
            try
            {
                var user = new ClienteManager().RetrieveByEmail(email);
                if (user == null)
                    return NotFound("Usuario no encontrado.");

                var emailVerifier = new EmailVerificationManager();
                emailVerifier.SendVerificationCode(email);
                return Ok("Código de recuperación enviado por email.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public ActionResult ResetPassword([FromBody] DTOs.PasswordResetRequest request)
        {
            try
            {
                var emailVerifier = new EmailVerificationManager();
                bool verified = emailVerifier.VerifyCode(request.email, request.Code);
                if (!verified)
                    return BadRequest("Código de verificación inválido.");

                var cm = new ClienteManager();
                var user = cm.RetrieveByEmail(request.email);
                if (user == null)
                    return NotFound("Usuario no encontrado.");

                user.contrasena = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                cm.Update(user);
                return Ok("Contraseña actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return false;
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}
