using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace WebApp.Pages
{
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
            // Validaci�n manual seg�n el tipo de usuario
            if (string.IsNullOrWhiteSpace(LoginRequest.UserType) || string.IsNullOrWhiteSpace(LoginRequest.Email))
            {
                ErrorMessage = "Debe ingresar el tipo de usuario y el correo.";
                return Page();
            }

            if (LoginRequest.UserType == "InstitucionBancaria")
            {
                if (string.IsNullOrWhiteSpace(LoginRequest.CedulaJuridica))
                {
                    ErrorMessage = "Debe ingresar la c�dula jur�dica.";
                    return Page();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(LoginRequest.Password))
                {
                    ErrorMessage = "Debe ingresar la contrase�a.";
                    return Page();
                }
            }

            string apiUrl = LoginRequest.UserType switch
            {
                "Cliente" => "https://localhost:5001/api/Cliente/Login",
                "Admin" => "https://localhost:5001/api/Admin/Login",
                "CuentaComercio" => "https://localhost:5001/api/CuentaComercio/Login",
                "InstitucionBancaria" => "https://localhost:5001/api/InstitucionBancaria/Login",
                _ => throw new Exception("Tipo de usuario no soportado")
            };

            using var httpClient = new HttpClient();

            object loginPayload = LoginRequest.UserType switch
            {
                "Admin" => new { UserName = LoginRequest.Email, Password = LoginRequest.Password },
                "CuentaComercio" => new { Email = LoginRequest.Email, Password = LoginRequest.Password },
                "InstitucionBancaria" => new { Email = LoginRequest.Email, CedulaJuridica = LoginRequest.CedulaJuridica },
                _ => new { Email = LoginRequest.Email, Password = LoginRequest.Password }
            };

            var json = JsonSerializer.Serialize(loginPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Usuario o credenciales incorrectos.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, LoginRequest.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                });

            if (LoginRequest.UserType == "Admin")
            {
                return RedirectToPage("/AdminHome");
            }
            return RedirectToPage("/AdminHome");
        }
    }

    public class LoginRequestModel
    {
        public string UserType { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? CedulaJuridica { get; set; }
    }
}
