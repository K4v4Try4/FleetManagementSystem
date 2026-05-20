using CarService.Shared.DTOs;
using CarService.Shared.Enums;

namespace CarService.Business.Interfaces
{
    /// <summary>
    /// Servizio di business per la gestione delle operazioni relative ai veicoli.
    /// </summary>
    /// <remarks>
    /// Questo layer contiene la logica applicativa che coordina repository,
    /// validazioni e regole di dominio per la gestione del "CarService".
    /// </remarks>
    public interface ICarBusinessService
    {
        /// <summary>
        /// Recupera tutte le auto attualmente disponibili.
        /// </summary>
        /// <returns>Una collezione di <see cref="CarDto"/> disponibili.</returns>
        Task<IEnumerable<CarDto>> GetAvailableCarsAsync();

        /// <summary>
        /// Recupera un veicolo tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco del veicolo.</param>
        /// <returns>
        /// Il <see cref="CarDto"/> corrispondente se trovato, altrimenti <c>null</c>.
        /// </returns>
        Task<CarDto?> GetCarByIdAsync(short id);

        /// <summary>
        /// Crea un nuovo veicolo nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari per la creazione del veicolo.</param>
        /// <returns>
        /// L'identificativo del veicolo creato.
        /// </returns>
        Task<short> CreateCarAsync(CreateCarDto dto);

        /// <summary>
        /// Aggiorna lo stato di un veicolo.
        /// </summary>
        /// <param name="id">Identificativo del veicolo.</param>
        /// <param name="status">Nuovo stato da assegnare.</param>
        Task UpdateStatusAsync(short id, string status);

        /// <summary>
        /// Elabora il completamento di un viaggio.
        /// </summary>
        /// <param name="carId">Identificativo del veicolo coinvolto.</param>
        /// <param name="kmTraveled">Chilometri percorsi durante il viaggio.</param>
        Task ProcessTripCompletionAsync(short carId, double kmTraveled);
    }
}