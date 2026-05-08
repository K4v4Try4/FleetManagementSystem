using EmployeeService.Shared.Entities;

namespace EmployeeService.Repository.Interfaces
{
    /// <summary>
    /// Interfaccia che definisce le operazioni di accesso ai dati per l'entità Employee.
    /// Implementa il pattern Repository per astrarre la persistenza dei dati.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Recupera tutti i dipendenti presenti nel sistema.
        /// </summary>
        /// <returns>Una collezione di Employee.</returns>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>
        /// Recupera un dipendente tramite il suo identificativo.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>Un Employee se trovato, altrimenti null.</returns>
        Task<Employee?> GetByIdAsync(short id);

        /// <summary>
        /// Aggiunge un nuovo dipendente al repository.
        /// </summary>
        /// <param name="employee">Entità Employee da inserire.</param>
        Task AddAsync(Employee employee);

        /// <summary>
        /// Aggiorna un dipendente esistente nel repository.
        /// </summary>
        /// <param name="employee">Entità Employee con dati aggiornati.</param>
        Task UpdateAsync(Employee employee);

        /// <summary>
        /// Persiste le modifiche effettuate nel contesto dati.
        /// </summary>
        Task SaveChangesAsync();
    }
}