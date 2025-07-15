using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;

namespace TodoProject.Controllers
{
    public class LanguageController : Controller
    {
        [HttpGet("set-language")]
        public IActionResult SetLanguage(string culture, string returnUrl = "/")
        {
            // Dil bilgisi çereze (cookie) yazılıyor
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );


            return LocalRedirect(returnUrl);
        }
    }
}
