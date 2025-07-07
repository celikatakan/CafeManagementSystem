using Microsoft.AspNetCore.Mvc;
using CafeManagementSystem.Web.Models;
using CafeManagementSystem.Web.Services;

namespace CafeManagementSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _api;

        public AccountController(ApiService api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _api.PostAsync<LoginResponse>("api/Auth/login", model);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Giriş başarısız.");
            }
            catch
            {
                ModelState.AddModelError("", "Sunucuya bağlanılamadı.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _api.PostAsync("api/Auth/register", model);
                return RedirectToAction("Login");
            }
            catch
            {
                ModelState.AddModelError("", "Kayıt başarısız.");
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }
    }
} 