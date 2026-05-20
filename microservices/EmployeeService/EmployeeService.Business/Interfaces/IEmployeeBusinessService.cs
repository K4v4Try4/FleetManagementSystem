using EmployeeService.Shared.DTOs;

namespace EmployeeService.Business.Interfaces
{
    /// <summary>
    /// Servizio di business per la gestione delle operazioni relative ai dipendenti.
    /// </summary>
    /// <remarks>
    /// Questo layer contiene la logica applicativa che coordina repository,
    /// validazioni e regole di dominio per la gestione del "EmployeeService".
    /// </remarks>
    public interface IEmployeeBusinessService
    {
        /// <summary>
        /// Recupera tutti i dipendenti presenti nel sistema.
        /// </summary>
        /// <returns>Una collezione di <see cref="EmployeeDto"/> disponibili.</returns>
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();

        /// <summary>
        /// Recupera un dipendente tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo univoco del dipendente.</param>
        /// <returns>
        /// Il <see cref="EmployeeDto"/> corrispondente se trovato, altrimenti <c>null</c>.
        /// </returns>
        Task<EmployeeDto?> GetEmployeeByIdAsync(short id);

        /// <summary>
        /// Crea un nuovo dipendente nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari per la creazione del dipendente.</param>
        /// <returns>
        /// L'identificativo del dipendente creato.
        /// </returns>
        Task<short> CreateEmployeeAsync(CreateEmployeeDto dto);

        /// <summary>
        /// Verifica se un dipendente è abilitato alla guida.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>True se il dipendente è abilitato, altrimenti false.</returns>
        Task<bool> IsEmployeeEligibleForDrivingAsync(short id);
    }
}