using BookingService.Shared.DTOs;

namespace BookingService.Business.Interfaces
{
    /// <summary>
    /// Servizio di business per la gestione delle operazioni relative alle prenotazioni.
    /// </summary>
    /// <remarks>
    /// Questo layer contiene la logica applicativa che coordina repository,
    /// validazioni e regole di dominio per la gestione del "BookingService".
    /// </remarks>
    public interface IBookingBusinessService
    {
        /// <summary>
        /// Recupera tutti le prenotazioni presenti nel sistema.
        /// </summary>
        /// <returns>Una collezione di <see cref="BookingDto"/> disponibili.</returns>
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();

        /// <summary>
        /// Recupera una prenotazione tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco della prenotazione.</param>
        /// <returns>
        /// Il <see cref="BookingDto"/> corrispondente se trovato, altrimenti <c>null</c>.
        /// </returns>
        Task<BookingDto?> GetBookingByIdAsync(short id);

        /// <summary>
        /// Crea una nuova prenotazione nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari per la creazione della prenotazione.</param>
        /// <returns>
        /// L'identificativo della prenotazione creata.
        /// </returns>
        Task<short> CreateBookingAsync(CreateBookingDto dto);

        /// <summary>
        /// Elabora il completamento di una prenotazione aggiornandone le informazioni finali.
        /// </summary>
        /// <param name="bookingId">
        /// Identificativo della prenotazione da completare.
        /// </param>
        /// <param name="kmTraveled">
        /// Chilometri percorsi durante la prenotazione.
        /// </param>
        /// <returns>
        /// Un task che rappresenta l'operazione asincrona.
        /// </returns>
        Task ProcessCompletionAsync(short bookingId, double kmTraveled);
    }
}