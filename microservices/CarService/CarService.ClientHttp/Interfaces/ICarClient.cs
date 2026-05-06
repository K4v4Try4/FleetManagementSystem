using CarService.Shared.DTOs;
using CarService.Shared.Enums;

namespace CarService.ClientHttp.Interfaces
{
    /// <summary>
    /// Client HTTP per l'interazione con il servizio CarService.
    /// </summary>
    /// <remarks>
    /// Questa interfaccia definisce le operazioni disponibili per comunicare
    /// con le API del servizio di gestione veicoli, tipicamente utilizzata
    /// da layer esterni (frontend, gateway o altri microservizi).
    /// </remarks>
    public interface ICarClient
    {
        /// <summary>
        /// Recupera l'elenco completo delle auto disponibili nel sistema.
        /// </summary>
        /// <returns>
        /// Una collezione di <see cref="CarDto"/> oppure <c>null</c> in caso di errore.
        /// </returns>
        /// <remarks>
        /// Utilizzato principalmente per la visualizzazione della flotta.
        /// </remarks>
        Task<IEnumerable<CarDto>?> GetCarsAsync();

        /// <summary>
        /// Recupera i dettagli di un veicolo tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco del veicolo.</param>
        /// <returns>
        /// Il <see cref="CarDto"/> corrispondente oppure <c>null</c> se non trovato.
        /// </returns>
        /// <remarks>
        /// Utilizzato per verifiche prima di operazioni come prenotazioni.
        /// </remarks>
        Task<CarDto?> GetCarByIdAsync(short id);

        /// <summary>
        /// Crea un nuovo veicolo nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione del veicolo.</param>
        /// <returns>
        /// Il <see cref="CreateCarDto"/> restituito dal servizio oppure <c>null</c> in caso di errore.
        /// </returns>
        Task<CreateCarDto?> CreateCarAsync(CreateCarDto dto);

        /// <summary>
        /// Aggiorna lo stato di un veicolo esistente.
        /// </summary>
        /// <param name="id">Identificativo del veicolo.</param>
        /// <param name="status">Nuovo stato da assegnare.</param>
        /// <returns>
        /// <c>true</c> se l'operazione è andata a buon fine, altrimenti <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Esempi di stato: AVAILABLE, IN_USE, MAINTENANCE.
        /// </remarks>
        Task<bool> UpdateCarStatusAsync(short id, string status);
    }
}