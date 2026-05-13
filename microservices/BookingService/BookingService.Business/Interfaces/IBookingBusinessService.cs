using BookingService.Shared.DTOs;

namespace BookingService.Business.Interfaces
{
    /// <summary>
    /// Definisce i servizi di business relativi alla gestione delle prenotazioni.
    /// </summary>
    public interface IBookingBusinessService
    {
        /// <summary>
        /// Recupera l'elenco completo delle prenotazioni.
        /// </summary>
        /// <returns>
        /// Una collezione di oggetti <see cref="BookingDto"/> contenenti i dati delle prenotazioni.
        /// </returns>
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();

        /// <summary>
        /// Recupera una prenotazione tramite il relativo identificativo.
        /// </summary>
        /// <param name="id">
        /// Identificativo univoco della prenotazione.
        /// </param>
        /// <returns>
        /// Un oggetto <see cref="BookingDto"/> se la prenotazione esiste;
        /// altrimenti <c>null</c>.
        /// </returns>
        Task<BookingDto?> GetBookingByIdAsync(short id);

        /// <summary>
        /// Crea una nuova prenotazione.
        /// </summary>
        /// <param name="dto">
        /// Oggetto contenente i dati necessari alla creazione della prenotazione.
        /// </param>
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