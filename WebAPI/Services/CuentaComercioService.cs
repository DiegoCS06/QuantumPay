using DTOs;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class CuentaComercioService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CuentaComercioService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // Crea una nueva cuenta de comercio
        public async Task CreateCuentaComercioAsync(CuentaComercio cuentaComercio)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt_token"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PostAsJsonAsync("api/CuentaComercio/Create", cuentaComercio);
            response.EnsureSuccessStatusCode();
        }

        // Verifica si la cuenta no tiene comercio asociado
        public async Task<bool> CuentaSinComercioAsync(int cuentaId)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt_token"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var cuenta = await _httpClient.GetFromJsonAsync<CuentaComercio>($"api/CuentaComercio/{cuentaId}");
                return cuenta != null && !cuenta.IdComercio.HasValue;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Si no existe la cuenta, se considera que no tiene comercio asociado
                return true;
            }
        }

        // Asocia un comercio a una cuenta
        public async Task AsociarComercioAsync(int cuentaId, int comercioId)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt_token"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new
            {
                CuentaId = cuentaId,
                ComercioId = comercioId
            };
            var response = await _httpClient.PostAsJsonAsync("api/CuentaComercio/asociar-comercio", request);
            response.EnsureSuccessStatusCode();
        }
    }
}