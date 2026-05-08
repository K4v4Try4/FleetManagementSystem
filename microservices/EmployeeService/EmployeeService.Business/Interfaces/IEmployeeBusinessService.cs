using EmployeeService.Shared.DTOs;

namespace EmployeeService.Business.Interfaces
{
    /// <summary>
    /// Interfaccia che definisce le operazioni di business per la gestione degli Employee.
    /// Astrazione del layer applicativo che incapsula la logica di dominio.
    /// </summary>
    public interface IEmployeeBusinessService
    {
        /// <summary>
        /// Recupera tutti i dipendenti presenti nel sistema.
        /// </summary>
        /// <returns>Collezione di EmployeeDto.</returns>
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();

        /// <summary>
        /// Recupera un dipendente tramite ID.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>EmployeeDto se trovato, altrimenti null.</returns>
        Task<EmployeeDto?> GetEmployeeByIdAsync(short id);

        /// <summary>
        /// Crea un nuovo dipendente nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione del dipendente.</param>
        /// <returns>Identificativo del dipendente creato.</returns>
        Task<short> CreateEmployeeAsync(CreateEmployeeDto dto);

        /// <summary>
        /// Verifica se un dipendente è idoneo alla guida.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>True se il dipendente è idoneo, altrimenti false.</returns>
        Task<bool> IsEmployeeEligibleForDrivingAsync(short id);
    }
}