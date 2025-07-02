using System;
namespace CafeManagementSystem.WebApi.Middlewares
{
	public static class MiddlewareExtensions
	{
        public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MaintenanceMiddleware>();
        }
    }
}

