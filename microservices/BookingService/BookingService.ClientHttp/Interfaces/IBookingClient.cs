using BookingService.Shared.DTOs;

namespace BookingService.ClientHttp.Interfaces
{
    /// <summary>
    /// Client HTTP per l'interazione con il servizio BookingService.
    /// </summary>
    /// <remarks>
    /// Questa interfaccia definisce le operazioni disponibili per comunicare
    /// con le API del servizio di gestione prenotazioni, tipicamente utilizzata
    /// da layer esterni.
    /// </remarks>
    public interface IBookingClient
    {
        /// <summary>
        /// Recupera la lista di tutte le prenotazioni dal servizio remoto.
        /// </summary>
        /// Una collezione di <see cref="BookingDto"/> oppure <c>null</c> in caso di errore.
        Task<IEnumerable<BookingDto>?> GetBookingsAsync();

        /// <summary>
        /// Recupera i dettagli di una prenotazione tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco della prenotazione.</param>
        /// <returns>
        /// Il <see cref="BookingDto"/> corrispondente oppure <c>null</c> se non trovato.
        /// </returns>
        Task<BookingDto?> GetBookingByIdAsync(short id);

        /// <summary>
        /// Crea una nuova prenotazione nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione della prenotazione.</param>
        /// <returns>
        /// Il <see cref="CreateBookingDto"/> restituito dal servizio oppure <c>null</c> in caso di errore.
        /// </returns>
        Task<short?> CreateBookingAsync(CreateBookingDto dto);

        /// <summary>
        /// Aggiorna lo stato di una prenotazione (es. per completarla o annullarla).
        /// </summary>
        /// <param name="id">ID della prenotazione.</param>
        /// <param name="kmTraveled">Chilometri percorsi (necessari se si completa il viaggio).</param>
        Task<bool> UpdateBookingStatusAsync(short id, double kmTraveled);
    }
}