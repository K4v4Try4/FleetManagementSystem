using CarService.Shared.Entities;

namespace CarService.Repository.Interfaces
{
    /// <summary>
    /// Repository per la gestione delle entità <see cref="Car"/>.
    /// </summary>
    /// <remarks>
    /// Espone le operazioni di accesso ai dati per la gestione delle auto.
    /// </remarks>
    public interface ICarRepository
    {
        /// <summary>
        /// Recupera tutte le auto presenti nel sistema.
        /// </summary>
        /// <returns>Una collezione di tutte le entità <see cref="Car"/>.</returns>
        Task<IEnumerable<Car>> GetAllAsync();

        /// <summary>
        /// Recupera un'auto tramite il suo identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco dell'auto.</param>
        /// <returns>
        /// L'entità <see cref="Car"/> se trovata, altrimenti <c>null</c>.
        /// </returns>
        Task<Car?> GetByIdAsync(short id);

        /// <summary>
        /// Aggiunge una nuova auto al sistema.
        /// </summary>
        /// <param name="car">Entità <see cref="Car"/> da inserire.</param>
        Task AddAsync(Car car);

        /// <summary>
        /// Aggiorna lo stato di un veicolo.
        /// </summary>
        /// <param name="id">Identificativo del veicolo da aggiornare.</param>
        /// <param name="status">Nuovo stato del veicolo.</param>
        Task UpdateStatusAsync(short id, string status);

        /// <summary>
        /// Aggiorna il chilometraggio di un veicolo.
        /// </summary>
        /// <param name="id">Identificativo del veicolo da aggiornare.</param>
        /// <param name="newMileage">Nuovo valore dei chilometri.</param>
        Task UpdateMileageAsync(short id, double newMileage);

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