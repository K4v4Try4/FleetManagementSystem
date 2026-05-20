using CarService.ClientHttp.Interfaces;
using CarService.Shared.DTOs;
using System.Net.Http.Json;

namespace CarService.ClientHttp.Implementations
{
    /// <summary>
    /// Implementazione del client HTTP per l'interazione con il servizio CarService.
    /// </summary>
    /// <remarks>
    /// Questa classe utilizza <see cref="HttpClient"/> per comunicare con le API REST
    /// del servizio di gestione veicoli.
    /// </remarks>
    public class CarClient : ICarClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Inizializza una nuova istanza del client HTTP.
        /// </summary>
        /// <param name="httpClient">
        /// Istanza di <see cref="HttpClient"/> configurata tramite dependency injection.
        /// </param>
        public CarClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CarDto>?> GetCarsAsync()
        {
            // GET /api/cars
            return await _httpClient.GetFromJsonAsync<IEnumerable<CarDto>>("api/cars");
        }

        /// <inheritdoc/>
        public async Task<CarDto?> GetCarByIdAsync(short id)
        {
            // GET /api/cars/{id}
            return await _httpClient.GetFromJsonAsync<CarDto>($"api/cars/{id}");
        }

        /// <inheritdoc/>
        public async Task<CreateCarDto?> CreateCarAsync(CreateCarDto dto)
        {
            // POST /api/cars
            var response = await _httpClient.PostAsJsonAsync("api/cars", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CreateCarDto>();
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCarStatusAsync(short id, string status)
        {
            // PATCH /api/cars/{id}/status
            var response = await _httpClient.PatchAsJsonAsync(
                $"api/cars/{id}/status",
                status);

            return response.IsSuccessStatusCode;
        }
    }
}