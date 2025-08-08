using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using DTOs;

namespace WebAPI.Services
{
    public class PromocionService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PromocionService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddJwtHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt_token"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<PromocionComercio>> GetPromocionesPorComercioAsync(int idComercio)
        {
            AddJwtHeader();
            var response = await _httpClient.GetAsync($"api/PromocionComercio/PorComercio/{idComercio}");
            if (!response.IsSuccessStatusCode) return new List<PromocionComercio>();
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await response.Content.ReadFromJsonAsync<List<PromocionComercio>>(options) ?? new List<PromocionComercio>();
        }

        public async Task CreatePromocionAsync(PromocionComercio promocion)
        {
            AddJwtHeader();
            var response = await _httpClient.PostAsJsonAsync("api/PromocionComercio/Create", promocion);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdatePromocionAsync(PromocionComercio promocion)
        {
            AddJwtHeader();
            var response = await _httpClient.PutAsJsonAsync("api/PromocionComercio/Update", promocion);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePromocionAsync(int id)
        {
            AddJwtHeader();
            var response = await _httpClient.DeleteAsync($"api/PromocionComercio/Delete/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}