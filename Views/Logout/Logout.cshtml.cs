using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DummyUIApp.Views.Logout
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = "/"
                },
                OpenIdConnectDefaults.AuthenticationScheme,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
