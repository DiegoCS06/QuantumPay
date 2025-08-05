// WebApp/Pages/Login.cshtml.cs
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginRequestModel LoginRequest { get; set; } = new LoginRequestModel();

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1) Validaciones básicas
            if (string.IsNullOrWhiteSpace(LoginRequest.UserType) ||
                string.IsNullOrWhiteSpace(LoginRequest.Email) ||
                string.IsNullOrWhiteSpace(LoginRequest.Password))
            {
                ErrorMessage = "Por favor ingrese tipo de usuario, correo y contraseña.";
                return Page();
            }

            // 2) Elegir la URL del API según UserType
            string apiUrl = LoginRequest.UserType switch
            {
                "Admin" => "https://localhost:5001/api/Admin/Login",
                "Cliente" => "https://localhost:5001/api/Cliente/Login",
                "CuentaComercio" => "https://localhost:5001/api/CuentaComercio/Login",
                "InstitucionBancaria" => "https://localhost:5001/api/InstitucionBancaria/Login",
                _ => throw new ArgumentException("Tipo de usuario no soportado")
            };

            // 3) Construir el payload según API
            object payload = LoginRequest.UserType switch
            {
                "Admin" => new { UserName = LoginRequest.Email, Password = LoginRequest.Password },
                _ => new { Email = LoginRequest.Email, Password = LoginRequest.Password }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(
                apiUrl,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Usuario o credenciales incorrectos.";
                return Page();
            }

            // 4) Leer la respuesta y extraer token y/o userId
            var body = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            string? token = null;
            if (root.TryGetProperty("token", out var tok))
                token = tok.GetString();

            int userId = 0;
            if (root.TryGetProperty("userId", out var idProp) ||
                root.TryGetProperty("UserId", out idProp))
            {
                userId = idProp.GetInt32();
            }

            // 5) Guardar JWT en cookie (para llamadas API)
            if (!string.IsNullOrEmpty(token))
            {
                Response.Cookies.Append("jwt_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    SameSite = SameSiteMode.Strict
                });
            }

            // 6) Construir Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,            LoginRequest.Email),
                new Claim(ClaimTypes.Role,            LoginRequest.UserType)
            };
            if (userId > 0)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            // 7) Agregar claims extra del JWT, si existe
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                foreach (var c in jwtToken.Claims)
                {
                    if (!claims.Exists(x => x.Type == c.Type && x.Value == c.Value))
                        claims.Add(c);
                }
            }

            // 8) Sign in con CookieAuthentication
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = true }
            );

            // 9) Redirigir según rol
            return LoginRequest.UserType switch
            {
                "Admin" => RedirectToPage("/AdminPages/AdminHome"),
                "Cliente" => RedirectToPage("/ClientesPages/ClienteHome"),
                "CuentaComercio" => RedirectToPage("/ComercioPages/ComercioHome"),
                "InstitucionBancaria" => RedirectToPage("/BancoPages/BancoHome"),
                _ => RedirectToPage("/")
            };
        }
    }

    public class LoginRequestModel
    {
        public string UserType { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
