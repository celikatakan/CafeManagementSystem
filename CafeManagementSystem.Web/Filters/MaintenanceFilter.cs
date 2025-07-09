using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using CafeManagementSystem.Web.Services;

namespace CafeManagementSystem.Web.Filters
{
    public class MaintenanceFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        //private readonly ApiService _apiService;

        //public MaintenanceFilter()
        //{
        //    var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        //    _apiService = new ApiService(new ConfigurationStub()); // Stub yerine gerçek config verirsen daha iyi olur
        //}

        //public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        //{
        //    var isMaintenance = await _apiService.IsMaintenanceModeAsync();

        //    var controller = filterContext.RouteData.Values["controller"].ToString();
        //    if (isMaintenance && !controller.Equals("Maintenance", StringComparison.OrdinalIgnoreCase))
        //    {
        //        filterContext.Result = new RedirectResult("~/Maintenance");
        //    }
        //    else
        //    {
        //        await next(); // devam et
        //    }
        //}
    }
} 