using EmployeeService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Repository.Persistence
{
    /// <summary>
    /// DbContext di Entity Framework Core per la gestione del modulo EmployeeService.
    /// </summary>
    public class EmployeeDbContext : DbContext
    {
        /// <summary>
        /// Inizializza una nuova istanza del contesto con le opzioni specificate.
        /// </summary>
        /// <param name="options">
        /// Opzioni di configurazione del DbContext.
        /// </param>
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        /// <summary>
        /// Rappresenta la tabella dei dipendenti nel database.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Configura il modello delle entità quando il contesto viene creato.
        /// </summary>
        /// <param name="modelBuilder">
        /// Builder utilizzato per configurare mapping, vincoli e conversioni.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                /// <summary>
                /// Definisce la chiave primaria.
                /// </summary>
                entity.HasKey(e => e.Id);

                /// <summary>
                /// Nome obbligartorio del dipendente con lunghezza massima di 100 caratteri.
                /// </summary>
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                /// <summary>
                /// Posizione obbligartoria del dipendente con lunghezza massima di 50 caratteri.
                /// </summary>
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);

                /// <summary>
                /// Indirizzo email obbligatorio del dipendente con lunghezza massima di 100 caratteri.
                /// </summary>
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

                /// <summary>
                /// Proprietà obbligatoria di abilita alla guida del dipendente.
                /// </summary>
                entity.Property(e => e.Eligibility).IsRequired();
            });
        }
    }
}