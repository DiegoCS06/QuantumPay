using CoreApp;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly JwtTokenService _jwtTokenService;
        public LoginController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] DTOs.LoginAdminRequest request)
        {
            try
            {
                var userName = string.Empty;
                var userPassword = string.Empty;
                var userId = 0;

                if (request.UserType == "Admin")
                {
                    var am = new AdminManager();
                    var admin = am.RetrieveByUserName(request.LoginName);

                    if (admin == null)
                    {
                        Console.WriteLine("[ADMIN LOGIN] Usuario no encontrado.");
                        return Unauthorized("Usuario o contraseña incorrectos.");
                    }
                    userName = admin.nombreUsuario;
                    userId = admin.Id;
                    userPassword = admin.contrasena;
                }

                if (request.UserType == "Cliente")
                {
                    var cm = new ClienteManager();
                    var cliente = cm.RetrieveByEmail(request.LoginName);

                    if (cliente == null)
                    {
                        Console.WriteLine("[ADMIN LOGIN] Usuario no encontrado.");
                        return Unauthorized("Usuario o contraseña incorrectos.");
                    }
                    userName = cliente.nombre + " " + cliente;
                    userId = cliente.Id;
                    userPassword = cliente.contrasena;
                }

                if (string.IsNullOrEmpty(userPassword))
                {
                    Console.WriteLine("[ADMIN LOGIN] Hash de contraseña vacío o nulo.");
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, userPassword))
                {
                    Console.WriteLine("[ADMIN LOGIN] Contraseña incorrecta.");
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }

                var token = _jwtTokenService.GenerateToken(userId, userName, request.UserType);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ADMIN LOGIN] Excepción: {ex.Message}");
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

        }
    }
}
