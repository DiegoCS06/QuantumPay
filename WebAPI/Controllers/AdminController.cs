using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BaseManager;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(Admin admin)
        {
            try
            {
                admin.contrasena = BCrypt.Net.BCrypt.HashPassword(admin.contrasena);

                var am = new AdminManager();
                await am.Create(admin);
                return Ok(admin);
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
                var am = new AdminManager();
                var lstResults = am.RetrieveAll();
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
                var am = new AdminManager();
                var result = am.RetrieveById(Id);
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
        [Route("RetrieveByUserName")]
        public ActionResult Retrieve(string userName)
        {
            try
            {
                var am = new AdminManager();
                var result = am.RetrieveByUserName(userName);

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
        public ActionResult Update(Admin admin)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(admin.contrasena))
                {
                    admin.contrasena = BCrypt.Net.BCrypt.HashPassword(admin.contrasena);
                }

                var cm = new AdminManager();
                var update = cm.Update(admin);
                return Ok(update);
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
                var cm = new AdminManager();
                var existing = cm.RetrieveById(id);
                cm.Delete(id);
                return Ok(new { Message = $"Admin con ID {id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] DTOs.LoginAdminRequest request)
        {
            try
            {
                Console.WriteLine($"[ADMIN LOGIN] UserName recibido: {request.UserName}");
                Console.WriteLine($"[ADMIN LOGIN] Password recibido: {request.Password}");

                var am = new AdminManager();
                var admin = am.RetrieveByUserName(request.UserName);

                if (admin == null)
                {
                    Console.WriteLine("[ADMIN LOGIN] Usuario no encontrado.");
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                Console.WriteLine($"[ADMIN LOGIN] Hash almacenado en BD: {admin.contrasena}");

                if (string.IsNullOrEmpty(admin.contrasena))
                {
                    Console.WriteLine("[ADMIN LOGIN] Hash de contraseña vacío o nulo.");
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, admin.contrasena))
                {
                    Console.WriteLine("[ADMIN LOGIN] Contraseña incorrecta.");
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                Console.WriteLine("[ADMIN LOGIN] Login exitoso.");
                return Ok(new { Message = "Login exitoso", UserId = admin.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ADMIN LOGIN] Excepción: {ex.Message}");
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
    }
}
