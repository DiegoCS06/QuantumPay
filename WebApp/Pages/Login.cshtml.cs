using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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
            // Validación manual según el tipo de usuario
            if (string.IsNullOrWhiteSpace(LoginRequest.UserType) || string.IsNullOrWhiteSpace(LoginRequest.Email))
            {
                ErrorMessage = "Debe ingresar el tipo de usuario y el correo.";
                return Page();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(LoginRequest.Password))
                {
                    ErrorMessage = "Debe ingresar la contraseña.";
                    return Page();
                }
            }

            string apiUrl = "https://localhost:5001/api/login";

            using var httpClient = new HttpClient();

            object loginPayload = new { LoginName = LoginRequest.Email, LoginRequest.Password, LoginRequest.UserType };

            var json = JsonSerializer.Serialize(loginPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
          
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Usuario o credenciales incorrectos.";
                return Page();
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(apiResponse);
            var root = jsonDoc.Deserialize<TokenResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(root.Token);

            var claims = new List<Claim>(jwtToken.Claims);
            //foreach (var claim in jwtToken.Claims)
            //{
                
            //    if (!claims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            //    {
            //        claims.Add(claim);
            //    }
            //}            

            Response.Cookies.Append("jwt_token", root.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                SameSite = SameSiteMode.Strict 
            });

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                });

            return LoginRequest.UserType switch
            {
                "Admin" => RedirectToPage("/AdminPages/AdminHome"),
                "Cliente" => RedirectToPage("/ClientesPages/ClienteHome"),
                "CuentaComercio" => RedirectToPage("/ComercioPages/ComercioHome"),
                "InstitucionBancaria" => RedirectToPage("/BancoPages/BancoHome"),
                _ => RedirectToPage("/") // en caso de tipo inesperado
            };
        }
    }

    public class LoginRequestModel
    {
        public string UserType { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
    }
    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
