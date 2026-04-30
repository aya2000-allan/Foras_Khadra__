using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Foras_Khadra.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Change(string culture, string returnUrl)
        {
            if (string.IsNullOrEmpty(culture))
                culture = "ar";

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                }
            );

            return LocalRedirect(returnUrl ?? "/");
        }
    }
}
