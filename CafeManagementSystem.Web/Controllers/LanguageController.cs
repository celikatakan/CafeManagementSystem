//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Localization;
//using Microsoft.Extensions.Localization;
//using CafeManagementSystem.Web.Resources;
//using System.Globalization;

//namespace CafeManagementSystem.Web.Controllers
//{
//    public class LanguageController : BaseController
//    {
//        public LanguageController(IStringLocalizer<SharedResource> localizer) : base(localizer)
//        {
//        }

//        [HttpPost]
//        public IActionResult SetLanguage(string culture, string returnUrl)
//        {
//            if (!string.IsNullOrEmpty(culture))
//            {
//                // Kültür geçerliliğini kontrol et
//                try
//                {
//                    var cultureInfo = new CultureInfo(culture);
//                }
//                catch (CultureNotFoundException)
//                {
//                    return BadRequest("Invalid culture");
//                }

//                // Cookie'yi ayarla
//                Response.Cookies.Append(
//                    CookieRequestCultureProvider.DefaultCookieName,
//                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
//                    new CookieOptions 
//                    { 
//                        Expires = DateTimeOffset.UtcNow.AddYears(1),
//                        IsEssential = true,
//                        SameSite = SameSiteMode.Lax,
//                        Secure = true,
//                        HttpOnly = true
//                    }
//                );

//                // Thread kültürünü güncelle
//                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
//                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
//            }

//            return LocalRedirect(returnUrl);
//        }
//    }
//} 