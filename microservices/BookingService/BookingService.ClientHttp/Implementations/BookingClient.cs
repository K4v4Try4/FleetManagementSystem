using BookingService.ClientHttp.Interfaces;
using BookingService.Shared.DTOs;
using System.Net.Http.Json;

namespace BookingService.ClientHttp.Implementations
{
    /// <summary>
    /// Implementazione del client HTTP per l'interazione con il servizio BookingService.
    /// </summary>
    /// <remarks>
    /// Questa classe utilizza <see cref="HttpClient"/> per comunicare con le API REST
    /// del servizio di gestione prenotazioni.
    /// </remarks>
    public class BookingClient : IBookingClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Inizializza una nuova istanza del client HTTP.
        /// </summary>
        /// <param name="httpClient">
        /// Istanza di <see cref="HttpClient"/> configurata tramite dependency injection.
        /// </param>
        public BookingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<BookingDto>?> GetBookingsAsync()
        {
            // GET /api/bookings
            return await _httpClient.GetFromJsonAsync<IEnumerable<BookingDto>>("api/bookings");
        }

        /// <inheritdoc/>
        public async Task<BookingDto?> GetBookingByIdAsync(short id)
        {
            // GET /api/bookings/{id}
            return await _httpClient.GetFromJsonAsync<BookingDto>($"api/bookings/{id}");
        }

        /// <inheritdoc/>
        public async Task<short?> CreateBookingAsync(CreateBookingDto dto)
        {
            // POST /api/bookings
            var response = await _httpClient.PostAsJsonAsync("api/bookings", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<short>();
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateBookingStatusAsync(short id, double kmTraveled)
        {
            // PATCH /api/bookings/{id}/status
            // Il body contiene i km percorsi per attivare la logica di completamento
            var response = await _httpClient.PatchAsJsonAsync($"api/bookings/{id}/status", kmTraveled);

            return response.IsSuccessStatusCode;
        }
    }
}