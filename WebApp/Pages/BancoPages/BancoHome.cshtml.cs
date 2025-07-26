using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.BancoPages
{
    [Authorize(Roles = "InstitucionBancaria")]
    public class BancoHomeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
