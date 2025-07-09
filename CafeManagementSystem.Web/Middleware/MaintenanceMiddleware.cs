using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace CafeManagementSystem.Web.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MaintenanceMiddleware> _logger;

        public MaintenanceMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, ILogger<MaintenanceMiddleware> logger)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                if (!context.Request.Path.StartsWithSegments("/Maintenance/Index"))
                {
                    try
                    {
                        var client = _httpClientFactory.CreateClient();
                        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
                        var apiBaseUrl = configuration["ApiSettings:BaseUrl"];
                        var url = $"{apiBaseUrl?.TrimEnd('/')}/api/settings/maintenance";
                        
                        _logger.LogInformation($"API isteği yapılıyor: {url}");
                        
                        var response = await client.GetAsync(url);
                        var json = await response.Content.ReadAsStringAsync();
                        
                        _logger.LogInformation($"API yanıtı: Status={response.StatusCode}, Content={json}");

                        // API 503 dönüyorsa veya başarılı yanıtta maintenance=true ise bakım modundadır
                        if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                        {
                            _logger.LogInformation("API 503 döndürdü, bakım sayfasına yönlendiriliyor");
                            context.Response.Redirect("/Maintenance/Index");
                            return;
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            var maintenanceStatus = IsMaintenanceEnabled(json);
                            if (maintenanceStatus)
                            {
                                _logger.LogInformation("Bakım modu aktif, yönlendirme yapılıyor: /Maintenance/Index");
                                context.Response.Redirect("/Maintenance/Index");
                                return;
                            }
                        }
                        else
                        {
                            _logger.LogWarning($"API beklenmeyen durum kodu döndürdü: {response.StatusCode}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogError($"API isteği başarısız: {ex.Message}");
                        // API'ye erişilemediğinde normal akışa devam et
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Middleware hatası: {ex.Message}");
                        // Diğer hatalarda normal akışa devam et
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Beklenmeyen hata: {ex.Message}");
                throw;
            }
        }

        private bool IsMaintenanceEnabled(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var maintenanceObj = JsonSerializer.Deserialize<MaintenanceStatusDto>(json, options);
                return maintenanceObj?.maintenance == true;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parse hatası: {ex.Message}");
                return false;
            }
        }

        private class MaintenanceStatusDto
        {
            public bool maintenance { get; set; }
        }
    }
}