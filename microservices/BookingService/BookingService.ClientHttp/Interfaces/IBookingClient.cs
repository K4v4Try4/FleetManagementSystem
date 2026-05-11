using BookingService.Shared.DTOs;

namespace BookingService.ClientHttp.Interfaces
{
    /// <summary>
    /// Client HTTP per l'interazione con il Booking Service.
    /// Permette a servizi esterni di gestire le prenotazioni.
    /// </summary>
    public interface IBookingClient
    {
        /// <summary>
        /// Recupera tutte le prenotazioni effettuate.
        /// </summary>
        Task<IEnumerable<BookingDto>?> GetBookingsAsync();

        /// <summary>
        /// Recupera il dettaglio di una singola prenotazione.
        /// </summary>
        Task<BookingDto?> GetBookingByIdAsync(short id);

        /// <summary>
        /// Crea una nuova prenotazione nel sistema.
        /// </summary>
        /// <returns>L'ID della prenotazione creata o null in caso di errore.</returns>
        Task<int?> CreateBookingAsync(CreateBookingDto dto);

        /// <summary>
        /// Aggiorna lo stato di una prenotazione (es. per completarla o annullarla).
        /// </summary>
        /// <param name="id">ID della prenotazione.</param>
        /// <param name="kmTraveled">Chilometri percorsi (necessari se si completa il viaggio).</param>
        Task<bool> UpdateBookingStatusAsync(short id, double kmTraveled);
    }
}