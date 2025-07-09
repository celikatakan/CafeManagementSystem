using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CafeManagementSystem.Web.Helpers;
using System.Security.Claims;

namespace CafeManagementSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                var userId = JwtHelper.GetUserIdFromToken(token);
                var (firstName, lastName, userType) = JwtHelper.GetUserInfoFromToken(token);
                var email = JwtHelper.GetEmailFromToken(token);

                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    ViewBag.UserFullName = $"{firstName} {lastName}";
                }
                ViewBag.UserType = userType;
                ViewBag.UserIsAdmin = userType == "Admin";
                ViewBag.UserIsCustomer = userType == "Customer";
                ViewBag.UserId = userId;

                // Set user claims
                var claims = new List<Claim>
                {
                    new Claim("id", userId.ToString()),
                    new Claim("email", email ?? ""),
                    new Claim("firstName", firstName ?? ""),
                    new Claim("lastName", lastName ?? ""),
                    new Claim("userType", userType ?? "Customer")
                };

                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);
                context.HttpContext.User = principal;
            }
            else
            {
                ViewBag.UserType = null;
                ViewBag.UserIsAdmin = false;
                ViewBag.UserIsCustomer = false;
            }
            base.OnActionExecuting(context);
        }
    }
} 