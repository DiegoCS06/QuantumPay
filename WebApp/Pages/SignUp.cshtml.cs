using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace WebApp.Pages
{
    [AllowAnonymous]
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public SignUpRequestModel SignUpRequest { get; set; } = new SignUpRequestModel();

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsValidForUserType(SignUpRequest, out string validationError))
            {
                ErrorMessage = validationError;
                return Page();
            }

            string apiUrl = SignUpRequest.UserType switch
            {
                "Cliente" => "https://localhost:5001/api/Cliente/Create",
                "Admin" => "https://localhost:5001/api/Admin/Create",
                "CuentaComercio" => "https://localhost:5001/api/CuentaComercio/Create",
                "InstitucionBancaria" => "https://localhost:5001/api/InstitucionBancaria/Create",
                _ => throw new Exception("Tipo de usuario no soportado")
            };

            using var httpClient = new HttpClient();

            object payload = SignUpRequest.UserType switch
            {
                "Cliente" => new {
                    nombre = SignUpRequest.Nombre,
                    apellido = SignUpRequest.Apellido,
                    cedula = SignUpRequest.Cedula,
                    telefono = SignUpRequest.Telefono,
                    correo = SignUpRequest.Correo,
                    direccion = SignUpRequest.Direccion,
                    contrasena = SignUpRequest.Password, // <-- aquí el cambio
                    IBAN = SignUpRequest.IBAN,
                    fotoCedula = SignUpRequest.FotoCedula,
                    fotoPerfil = SignUpRequest.FotoPerfil,
                    fechaNacimiento = SignUpRequest.FechaNacimiento
                },
                "Admin" => new {
                    nombreUsuario = SignUpRequest.NombreUsuario,
                    contrasena = SignUpRequest.Password // <-- aquí el cambio
                },
                "CuentaComercio" => new {
                    nombreUsuario = SignUpRequest.NombreUsuario,
                    contrasena = SignUpRequest.Password, // <-- aquí el cambio
                    cedulaJuridica = SignUpRequest.CedulaJuridica,
                    telefono = SignUpRequest.Telefono,
                    correoElectronico = SignUpRequest.CorreoElectronico,
                    direccion = SignUpRequest.Direccion
                },
                "InstitucionBancaria" => new {
                    codigoIdentidad = SignUpRequest.CodigoIdentidad,
                    codigoIBAN = SignUpRequest.CodigoIBAN,
                    cedulaJuridica = SignUpRequest.CedulaJuridica,
                    direccionSedePrincipal = SignUpRequest.DireccionSedePrincipal,
                    telefono = SignUpRequest.Telefono,
                    correoElectronico = SignUpRequest.CorreoElectronico
                },
                _ => throw new Exception("Tipo de usuario no soportado")
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Error al registrar el usuario.";
                return Page();
            }

            SuccessMessage = "Registro exitoso. Ahora puede iniciar sesión.";
            return RedirectToPage("/Login");
        }

        private bool IsValidForUserType(SignUpRequestModel req, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(req.UserType))
            {
                error = "Debe seleccionar un tipo de usuario.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(req.Password) || req.Password.Length < 6)
            {
                error = "La contraseña es obligatoria y debe tener al menos 6 caracteres.";
                return false;
            }

            switch (req.UserType)
            {
                case "Cliente":
                    if (string.IsNullOrWhiteSpace(req.Nombre) ||
                        string.IsNullOrWhiteSpace(req.Apellido) ||
                        string.IsNullOrWhiteSpace(req.Cedula) ||
                        req.Telefono == null ||
                        string.IsNullOrWhiteSpace(req.Correo) ||
                        string.IsNullOrWhiteSpace(req.Direccion) ||
                        string.IsNullOrWhiteSpace(req.FotoCedula) ||
                        string.IsNullOrWhiteSpace(req.FotoPerfil) ||
                        req.FechaNacimiento == null)
                    {
                        error = "Todos los campos de cliente son obligatorios.";
                        return false;
                    }
                    break;
                case "Admin":
                    if (string.IsNullOrWhiteSpace(req.NombreUsuario))
                    {
                        error = "El nombre de usuario es obligatorio para el administrador.";
                        return false;
                    }
                    break;
                case "CuentaComercio":
                    if (string.IsNullOrWhiteSpace(req.NombreUsuario) ||
                        string.IsNullOrWhiteSpace(req.CedulaJuridica) ||
                        req.Telefono == null ||
                        string.IsNullOrWhiteSpace(req.CorreoElectronico) ||
                        string.IsNullOrWhiteSpace(req.Direccion))
                    {
                        error = "Todos los campos de comercio son obligatorios.";
                        return false;
                    }
                    break;
                case "InstitucionBancaria":
                    if (string.IsNullOrWhiteSpace(req.CodigoIdentidad) ||
                        string.IsNullOrWhiteSpace(req.CodigoIBAN) ||
                        string.IsNullOrWhiteSpace(req.CedulaJuridica) ||
                        string.IsNullOrWhiteSpace(req.DireccionSedePrincipal) ||
                        req.Telefono == null ||
                        string.IsNullOrWhiteSpace(req.CorreoElectronico))
                    {
                        error = "Todos los campos de institución bancaria son obligatorios.";
                        return false;
                    }
                    break;
                default:
                    error = "Tipo de usuario no soportado.";
                    return false;
            }
            return true;
        }
    }

    public class SignUpRequestModel
    {
        public string UserType { get; set; }
        public string Password { get; set; }

        // Cliente
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public int? Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string FotoCedula { get; set; } // base64 o url
        public string FotoPerfil { get; set; } // base64 o url
        public DateTime? FechaNacimiento { get; set; }
        public string IBAN { get; set; }

        // Admin
        public string NombreUsuario { get; set; }

        // CuentaComercio
        public string CedulaJuridica { get; set; }
        public string CorreoElectronico { get; set; }

        // InstitucionBancaria
        public string CodigoIdentidad { get; set; }
        public string CodigoIBAN { get; set; }
        public string DireccionSedePrincipal { get; set; }
        public string EstadoSolicitud { get; set; }
    }
}