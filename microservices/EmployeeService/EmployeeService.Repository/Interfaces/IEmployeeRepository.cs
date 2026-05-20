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
        /// <returns>Una collezione di tutte le entità <see cref="Employee"/>.</returns>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>
        /// Recupera un dipendente tramite il suo identificativo.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// L'entità <see cref="Employee"/> se trovata, altrimenti <c>null</c>.
        Task<Employee?> GetByIdAsync(short id);

        /// <summary>
        /// Aggiunge un nuovo dipendente al sistema.
        /// </summary>
        /// <param name="car">Entità <see cref="Employee"/> da inserire.</param>
        Task AddAsync(Employee employee);

        /// <summary>
        /// Aggiorna un dipendente esistente nel repository.
        /// </summary>
        /// <param name="car">Entità <see cref="Employee"/> da aggiornare.</param>
        Task UpdateAsync(Employee employee);

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