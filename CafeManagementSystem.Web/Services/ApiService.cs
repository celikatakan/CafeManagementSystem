using System;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace CafeManagementSystem.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7265/";
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");

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
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}{endpoint}", data);

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
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}{endpoint}", data);

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
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}{endpoint}", data);

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
                var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}");

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
