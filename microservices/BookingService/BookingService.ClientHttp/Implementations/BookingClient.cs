using BookingService.ClientHttp.Interfaces;
using BookingService.Shared.DTOs;
using System.Net.Http.Json;

namespace BookingService.ClientHttp.Implementations
{
    /// <summary>
    /// Implementazione del client HTTP per l'interazione con le API di BookingService.
    /// Incapsula le chiamate REST verso il servizio di prenotazioni.
    /// </summary>
    public class BookingClient : IBookingClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Inizializza una nuova istanza di <see cref="BookingClient"/>.
        /// </summary>
        /// <param name="httpClient">
        /// Client HTTP utilizzato per effettuare le richieste verso il BookingService.
        /// </param>
        public BookingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Recupera tutte le prenotazioni disponibili dal servizio remoto.
        /// </summary>
        /// <returns>
        /// Una collezione di <see cref="BookingDto"/> oppure <c>null</c> in caso di errore.
        /// </returns>
        public async Task<IEnumerable<BookingDto>?> GetBookingsAsync()
        {
            // GET /api/bookings
            return await _httpClient.GetFromJsonAsync<IEnumerable<BookingDto>>("api/bookings");
        }

        /// <summary>
        /// Recupera una prenotazione specifica tramite identificativo.
        /// </summary>
        /// <param name="id">
        /// Identificativo della prenotazione.
        /// </param>
        /// <returns>
        /// Un oggetto <see cref="BookingDto"/> oppure <c>null</c> se non trovato o in caso di errore.
        /// </returns>
        public async Task<BookingDto?> GetBookingByIdAsync(short id)
        {
            // GET /api/bookings/{id}
            return await _httpClient.GetFromJsonAsync<BookingDto>($"api/bookings/{id}");
        }

        /// <summary>
        /// Crea una nuova prenotazione tramite API remota.
        /// </summary>
        /// <param name="dto">
        /// Dati necessari alla creazione della prenotazione.
        /// </param>
        /// <returns>
        /// L'identificativo della prenotazione creata oppure <c>null</c> in caso di errore.
        /// </returns>
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

        /// <summary>
        /// Aggiorna lo stato di una prenotazione, includendo i chilometri percorsi.
        /// </summary>
        /// <param name="id">
        /// Identificativo della prenotazione.
        /// </param>
        /// <param name="kmTraveled">
        /// Chilometri percorsi durante la prenotazione.
        /// </param>
        /// <returns>
        /// <c>true</c> se l'aggiornamento è andato a buon fine; altrimenti <c>false</c>.
        /// </returns>
        public async Task<bool> UpdateBookingStatusAsync(short id, double kmTraveled)
        {
            // PATCH /api/bookings/{id}/status
            // Il body contiene i km percorsi per attivare la logica di completamento
            var response = await _httpClient.PatchAsJsonAsync(
                $"api/bookings/{id}/status",
                kmTraveled);

            return response.IsSuccessStatusCode;
        }
    }
}