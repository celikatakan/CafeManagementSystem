using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CafeManagementSystem.Web.Helpers;

namespace CafeManagementSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                var (firstName, lastName, userType) = JwtHelper.GetUserInfoFromToken(token);
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    ViewBag.UserFullName = $"{firstName} {lastName}";
                }
                ViewBag.UserType = userType; // "Admin" veya "Customer" gibi
                ViewBag.UserIsAdmin = userType == "Admin";
                ViewBag.UserIsCustomer = userType == "Customer";

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