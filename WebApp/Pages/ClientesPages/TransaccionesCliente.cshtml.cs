using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace WebApp.Pages.ClientesPages
{
    [Authorize(Roles = "Cliente")]
    public class TransaccionesClienteModel : PageModel
    {
        public string Token => HttpContext.Session.GetString("Token");
        public void OnGet()
        {
           
        }
    }
}
