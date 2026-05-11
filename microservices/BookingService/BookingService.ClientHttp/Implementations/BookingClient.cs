using BookingService.ClientHttp.Interfaces;
using BookingService.Shared.DTOs;
using System.Net.Http.Json;

namespace BookingService.ClientHttp.Implementations
{
    public class BookingClient : IBookingClient
    {
        private readonly HttpClient _httpClient;

        public BookingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BookingDto>?> GetBookingsAsync()
        {
            // GET /api/bookings
            return await _httpClient.GetFromJsonAsync<IEnumerable<BookingDto>>("api/bookings");
        }

        public async Task<BookingDto?> GetBookingByIdAsync(short id)
        {
            // GET /api/bookings/{id}
            return await _httpClient.GetFromJsonAsync<BookingDto>($"api/bookings/{id}");
        }

        public async Task<int?> CreateBookingAsync(CreateBookingDto dto)
        {
            // POST /api/bookings
            var response = await _httpClient.PostAsJsonAsync("api/bookings", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            return null;
        }

        public async Task<bool> UpdateBookingStatusAsync(short id, double kmTraveled)
        {
            // PATCH /api/bookings/{id}/status
            // Passiamo i KM percorsi nel body per permettere al servizio di scatenare l'evento TripCompleted
            var response = await _httpClient.PatchAsJsonAsync($"api/bookings/{id}/status", kmTraveled);
            return response.IsSuccessStatusCode;
        }
    }
}