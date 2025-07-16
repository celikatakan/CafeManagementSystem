using System;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace CafeManagementSystem.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7265/";
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddJwtHeader(HttpRequestMessage request)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}{endpoint}");
                AddJwtHeader(request);
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new MaintenanceException("Sistem bakım modunda.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiException($"API isteği başarısız oldu. Status: {response.StatusCode}");
                }

                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API'ye erişilemiyor.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("API yanıtı işlenemedi.", ex);
            }
        }

        public async Task PostAsync(string endpoint, object data)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{endpoint}")
                {
                    Content = JsonContent.Create(data)
                };
                AddJwtHeader(request);
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new MaintenanceException("Sistem bakım modunda.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ApiException($"API isteği başarısız oldu. Status: {response.StatusCode}, Content: {content}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API'ye erişilemiyor.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("İstek verisi işlenemedi.", ex);
            }
        }

        public async Task<TResponse> PostAsync<TResponse>(string endpoint, object data)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{endpoint}")
                {
                    Content = JsonContent.Create(data)
                };
                AddJwtHeader(request);
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new MaintenanceException("Sistem bakım modunda.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ApiException($"API isteği başarısız oldu. Status: {response.StatusCode}, Content: {content}");
                }

                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API'ye erişilemiyor.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("API yanıtı işlenemedi.", ex);
            }
        }

        public async Task PutAsync(string endpoint, object data)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}{endpoint}")
                {
                    Content = JsonContent.Create(data)
                };
                AddJwtHeader(request);
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new MaintenanceException("Sistem bakım modunda.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ApiException($"API isteği başarısız oldu. Status: {response.StatusCode}, Content: {content}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API'ye erişilemiyor.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException("İstek verisi işlenemedi.", ex);
            }
        }

        public async Task DeleteAsync(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}{endpoint}");
                AddJwtHeader(request);
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new MaintenanceException("Sistem bakım modunda.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiException($"API isteği başarısız oldu. Status: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API'ye erişilemiyor.", ex);
            }
        }

        public async Task PatchAsync(string endpoint, object data = null)
        {
            try
            {
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_baseUrl}{endpoint}")
                {
                    Content = data != null ? JsonContent.Create(data) : null
                };
                AddJwtHeader(request);
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new MaintenanceException("Sistem bakım modunda.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ApiException($"API isteği başarısız oldu. Status: {response.StatusCode}, Content: {content}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("API'ye erişilemiyor.", ex);
            }
        }

        public class ApiException : Exception
        {
            public ApiException(string message) : base(message) { }
            public ApiException(string message, Exception innerException) : base(message, innerException) { }
        }

        public class MaintenanceException : Exception
        {
            public MaintenanceException(string message) : base(message) { }
        }
    }
}
