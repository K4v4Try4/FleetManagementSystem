using EmployeeService.Repository.Interfaces;
using EmployeeService.Repository.Persistence;
using EmployeeService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Repository.Implementations
{
    /// <summary>
    /// Implementazione concreta del repository per l'entità Employee.
    /// Utilizza Entity Framework Core per la persistenza dei dati.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        /// <summary>
        /// Inizializza una nuova istanza del repository.
        /// </summary>
        /// <param name="context">Contesto EF Core per l'accesso al database.</param>
        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recupera tutti i dipendenti dal database.
        /// </summary>
        /// <returns>Collezione di Employee.</returns>
        public async Task<IEnumerable<Employee>> GetAllAsync() => await _context.Employees.ToListAsync();

        /// <summary>
        /// Recupera un dipendente tramite ID.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>Employee se trovato, altrimenti null.</returns>
        public async Task<Employee?> GetByIdAsync(short id) => await _context.Employees.FindAsync(id);

        /// <summary>
        /// Aggiunge un nuovo dipendente al contesto di persistenza.
        /// </summary>
        /// <param name="employee">Entità da inserire.</param>
        public async Task AddAsync(Employee employee) => await _context.Employees.AddAsync(employee);

        /// <summary>
        /// Aggiorna un dipendente esistente nel contesto.
        /// </summary>
        /// <param name="employee">Entità con valori aggiornati.</param>
        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Salva tutte le modifiche pendenti nel database.
        /// </summary>
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}