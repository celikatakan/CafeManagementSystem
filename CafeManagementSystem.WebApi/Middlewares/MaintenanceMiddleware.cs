using System;
using CafeManagementSystem.Business.Operations.Setting;

namespace CafeManagementSystem.WebApi.Middlewares
{
	public class MaintenanceMiddleware
	{
        private readonly RequestDelegate _next;


        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var settingService = context.RequestServices.GetRequiredService<ISettingService>();
            bool maintenanceMode = settingService.GetMaintenanceState();
            if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context);
                return;
            }

            if (maintenanceMode)
            {
                context.Response.StatusCode = 503;
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new {
                    error = "Bakımdayız",
                    message = "Şu an hizmet verememekteyiz."
                });
                await context.Response.WriteAsync(result);
                return;
            }
            else
                await _next(context);
        }
    }
}

