using BookingService.Shared.Entities;

namespace BookingService.Repository.Interfaces
{
    /// <summary>
    /// Repository per la gestione delle entità <see cref="Booking"/>.
    /// </summary>
    /// <remarks>
    /// Espone le operazioni di accesso ai dati per la gestione delle prenotazioni.
    /// </remarks>
    public interface IBookingRepository
    {
        /// <summary>
        /// Recupera una prenotazione tramite il suo identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco della prenotazione.</param>
        /// <returns>
        /// L'entità <see cref="Booking"/> se trovata, altrimenti <c>null</c>.
        /// </returns>
        Task<Booking> GetByIdAsync(short id);

        /// <summary>
        /// Recupera tutte le prenotazioni presenti nel sistema.
        /// </summary>
        /// <returns>Una collezione di tutte le entità <see cref="Booking"/>.</returns>
        Task<IEnumerable<Booking>> GetAllAsync();

        /// <summary>
        /// Aggiunge una nuova prenotazione al sistema.
        /// </summary>
        /// <param name="booking">Entità <see cref="Booking"/> da inserire.</param>
        Task AddAsync(Booking booking);

        /// <summary>
        /// Aggiorna una prenotazione esistente.
        /// </summary>
        /// <param name="booking">Entità da aggiornare.</param>
        Task UpdateAsync(Booking booking);

        /// <summary>
        /// Verifica se un impiegato ha già una prenotazione attiva.
        /// </summary>
        /// <param name="employeeId">Identificativo dell'impiegato.</param>
        /// <returns>
        /// <c>true</c> se esiste una prenotazione attiva; altrimenti <c>false</c>.
        /// </returns>
        Task<bool> HasActiveBookingAsync(short employeeId);

        /// <summary>
        /// Persiste tutte le modifiche pendenti sul database.
        /// </summary>
        /// <remarks>
        /// Deve essere chiamato dopo operazioni di inserimento o aggiornamento
        /// per rendere persistenti le modifiche.
        /// </remarks>
        Task SaveChangesAsync();
    }
}