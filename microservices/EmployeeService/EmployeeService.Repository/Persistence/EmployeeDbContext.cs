using EmployeeService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Repository.Persistence
{
    /// <summary>
    /// Contesto del database per il servizio Employee.
    /// Gestisce la configurazione delle entità e la connessione al database tramite Entity Framework Core.
    /// </summary>
    public class EmployeeDbContext : DbContext
    {
        /// <summary>
        /// Inizializza una nuova istanza del contesto.
        /// </summary>
        /// <param name="options">Opzioni di configurazione del DbContext.</param>
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        /// <summary>
        /// Tabella degli Employee nel database.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Configura il modello delle entità e le relative regole di mapping.
        /// </summary>
        /// <param name="modelBuilder">Builder utilizzato per configurare le entità.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                /// Configurazione chiave primaria
                entity.HasKey(e => e.Id);

                /// Configurazione proprietà Name
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                /// Configurazione proprietà Role
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);

                /// Configurazione proprietà Email
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

                /// Configurazione proprietà Eligibility
                entity.Property(e => e.Eligibility).IsRequired();
            });
        }
    }
}