using BookingService.Shared.Entities;

namespace BookingService.Repository.Interfaces
{
    /// <summary>
    /// Definisce le operazioni di accesso ai dati per l'entità <see cref="Booking"/>.
    /// Espone metodi per operazioni CRUD e query specifiche.
    /// </summary>
    public interface IBookingRepository
    {
        /// <summary>
        /// Recupera una prenotazione tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo della prenotazione.</param>
        /// <returns>
        /// L'entità <see cref="Booking"/> se trovata; altrimenti <c>null</c>.
        /// </returns>
        Task<Booking> GetByIdAsync(short id);

        /// <summary>
        /// Recupera tutte le prenotazioni presenti nel sistema.
        /// </summary>
        /// <returns>Collezione di entità <see cref="Booking"/>.</returns>
        Task<IEnumerable<Booking>> GetAllAsync();

        /// <summary>
        /// Aggiunge una nuova prenotazione al repository.
        /// </summary>
        /// <param name="booking">Entità da inserire.</param>
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
        /// Persiste le modifiche pendenti sul database.
        /// </summary>
        Task SaveChangesAsync();
    }
}