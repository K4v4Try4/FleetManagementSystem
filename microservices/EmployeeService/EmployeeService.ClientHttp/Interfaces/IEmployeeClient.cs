using EmployeeService.Shared.DTOs;

namespace EmployeeService.ClientHttp.Interfaces
{
    /// <summary>
    /// Client HTTP per l'interazione con il servizio Employee.
    /// Incapsula le chiamate verso le API del microservizio.
    /// </summary>
    public interface IEmployeeClient
    {
        /// <summary>
        /// Recupera la lista di tutti i dipendenti dal servizio remoto.
        /// </summary>
        /// <returns>Collezione di EmployeeDto oppure null in caso di errore.</returns>
        Task<IEnumerable<EmployeeDto>?> GetEmployeesAsync();

        /// <summary>
        /// Recupera un dipendente tramite ID.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>EmployeeDto se trovato, altrimenti null.</returns>
        Task<EmployeeDto?> GetEmployeeByIdAsync(short id);

        /// <summary>
        /// Verifica l'idoneità alla guida di un dipendente.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>True se idoneo, altrimenti false.</returns>
        Task<bool> GetEmployeeEligibilityAsync(short id);

        /// <summary>
        /// Crea un nuovo dipendente tramite API remota.
        /// </summary>
        /// <param name="dto">Dati del dipendente da creare.</param>
        /// <returns>Identificativo del dipendente creato oppure null in caso di errore.</returns>
        Task<short?> CreateEmployeeAsync(CreateEmployeeDto dto);
    }
}