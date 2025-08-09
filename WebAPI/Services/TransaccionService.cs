using DTOs;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace WebAPI.Services
{
    public class TransaccionService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransaccionService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Transaccion>> GetTransaccionesPorComercioAsync(int idComercio)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt_token"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"api/Transaccion/RetrieveByComercio?idComercio={idComercio}");
            if (!response.IsSuccessStatusCode) return new List<Transaccion>();
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await response.Content.ReadFromJsonAsync<List<Transaccion>>(options) ?? new List<Transaccion>();
        }
    }
}