using BookingService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Persistence
{
    /// <summary>
    /// DbContext di Entity Framework Core per la gestione del modulo BookingService.
    /// </summary>
    public class BookingDbContext : DbContext
    {
        /// <summary>
        /// Inizializza una nuova istanza del contesto con le opzioni specificate.
        /// </summary>
        /// <param name="options">
        /// Opzioni di configurazione del DbContext.
        /// </param>
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        /// <summary>
        /// Tabella delle prenotazioni nel database.
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Configura il modello EF Core e le relative mapping verso il database.
        /// </summary>
        /// <param name="modelBuilder">
        /// Builder utilizzato per configurare entità, relazioni e indici.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione entità Booking
            modelBuilder.Entity<Booking>(entity =>
            {
                /// <summary>
                /// Definisce la chiave primaria.
                /// </summary>
                entity.HasKey(e => e.Id);

                /// <summary>
                /// Identificativo obbligatorio del dipendente.
                /// </summary>
                entity.Property(e => e.EmployeeId).IsRequired();

                /// <summary>
                /// Identificativo obbligatorio dell'auto.
                /// </summary>
                entity.Property(e => e.CarId).IsRequired();

                /// <summary>
                /// Ora di inizio obbligatoria.
                /// </summary>
                entity.Property(e => e.StartTime).IsRequired();

                /// <summary>
                /// Ora di fine obbligatoria.
                /// </summary>
                entity.Property(e => e.EndTime).IsRequired();

                /// <summary>
                /// Stato della prenotazione obbligatoria e salvato come stringa nel database.
                /// </summary>
                entity.Property(e => e.Status).IsRequired().HasConversion<string>();

                // Indici per ottimizzare le query più frequenti
                entity.HasIndex(e => e.EmployeeId);
                entity.HasIndex(e => e.CarId);
            });
        }
    }
}