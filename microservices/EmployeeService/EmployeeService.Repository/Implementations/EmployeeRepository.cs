using EmployeeService.Repository.Interfaces;
using EmployeeService.Repository.Persistence;
using EmployeeService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Repository.Implementations
{
    /// <summary>
    /// Implementazione del repository per la gestione delle entità <see cref="Employee"/>.
    /// </summary>
    /// <remarks>
    /// Implementa le operazioni definite in <see cref="IEmployeeRepository"/>.
    /// </remarks>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        /// <summary>
        /// Inizializza una nuova istanza del repository.
        /// </summary>
        /// <param name="context">
        /// Contesto EF Core utilizzato per l'accesso al database.
        /// </param>
        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Employee>> GetAllAsync() => await _context.Employees.ToListAsync();

        /// <inheritdoc/>
        public async Task<Employee?> GetByIdAsync(short id) => await _context.Employees.FindAsync(id);

        /// <inheritdoc/>
        public async Task AddAsync(Employee employee) => await _context.Employees.AddAsync(employee);

        /// <inheritdoc/>
        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}