using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace WebApplication4.Filters
{
    public class CultureFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var culture = context.HttpContext.Request.Cookies["lang"] ?? "ru";
            var cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
