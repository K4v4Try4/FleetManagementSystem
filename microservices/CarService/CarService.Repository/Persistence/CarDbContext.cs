using CarService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.Repository.Persistence
{
    /// <summary>
    /// DbContext di Entity Framework Core per la gestione del modulo CarService.
    /// </summary>
    public class CarDbContext : DbContext
    {
        /// <summary>
        /// Inizializza una nuova istanza del contesto con le opzioni specificate.
        /// </summary>
        /// <param name="options">
        /// Opzioni di configurazione del DbContext.
        /// </param>
        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options) { }

        /// <summary>
        /// Rappresenta la tabella dei veicoli nel database.
        /// </summary>
        public DbSet<Car> Cars { get; set; }

        /// <summary>
        /// Configura il modello delle entità quando il contesto viene creato.
        /// </summary>
        /// <param name="modelBuilder">
        /// Builder utilizzato per configurare mapping, vincoli e conversioni.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurazione della tabella Car
            modelBuilder.Entity<Car>(entity =>
            {
                /// <summary>
                /// Definisce la chiave primaria.
                /// </summary>
                entity.HasKey(e => e.Id);

                /// <summary>
                /// Numero di targa obbligatorio con lunghezza massima 20 caratteri.
                /// </summary>
                entity.Property(e => e.PlateNumber).IsRequired().HasMaxLength(20);

                /// <summary>
                /// Modello del veicolo obbligatorio con lunghezza massima 100 caratteri.
                /// </summary>
                entity.Property(e => e.Model).IsRequired().HasMaxLength(100);

                /// <summary>
                /// Stato del veicolo obbligatorio e salvato come stringa nel database.
                /// </summary>
                entity.Property(e => e.Status).IsRequired().HasConversion<string>();

                /// <summary>
                /// Chilometraggio del veicolo obbligatorio.
                /// </summary>
                entity.Property(e => e.Mileage).IsRequired();
            });
        }
    }
}