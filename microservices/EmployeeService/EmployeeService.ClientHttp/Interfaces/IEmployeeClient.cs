using EmployeeService.Shared.DTOs;

namespace EmployeeService.ClientHttp.Interfaces
{
    /// <summary>
    /// Client HTTP per l'interazione con il servizio EmployeeService.
    /// </summary>
    /// <remarks>
    /// Questa interfaccia definisce le operazioni disponibili per comunicare
    /// con le API del servizio di gestione dipendenti, tipicamente utilizzata
    /// da layer esterni.
    /// </remarks>
    public interface IEmployeeClient
    {
        /// <summary>
        /// Recupera la lista di tutti i dipendenti dal servizio remoto.
        /// </summary>
        /// Una collezione di <see cref="EmployeeDto"/> oppure <c>null</c> in caso di errore.
        Task<IEnumerable<EmployeeDto>?> GetEmployeesAsync();

        /// <summary>
        /// Recupera i dettagli di un dipendente tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco del dipendente.</param>
        /// <returns>
        /// Il <see cref="EmployeeDto"/> corrispondente oppure <c>null</c> se non trovato.
        /// </returns>
        Task<EmployeeDto?> GetEmployeeByIdAsync(short id);

        /// <summary>
        /// Verifica l'idoneità alla guida di un dipendente.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>True se idoneo, altrimenti false.</returns>
        Task<bool> GetEmployeeEligibilityAsync(short id);

        /// <summary>
        /// Crea un nuovo dipendente nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione del dipendente.</param>
        /// <returns>
        /// Il <see cref="CreateEmployeeDto"/> restituito dal servizio oppure <c>null</c> in caso di errore.
        /// </returns>
        Task<short?> CreateEmployeeAsync(CreateEmployeeDto dto);
    }
}