using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    public class LocalizationController : Controller
    {
        [HttpPost]
        public IActionResult ChangeCulture(string lang)
        {
            var supportedLanguages = new[] { "ru", "en", "ja", "es", "ro" };
            var selectedLang = supportedLanguages.Contains(lang) ? lang : "uk";
            Response.Cookies.Append("lang", selectedLang, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(10),
                IsEssential = true
            });

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
