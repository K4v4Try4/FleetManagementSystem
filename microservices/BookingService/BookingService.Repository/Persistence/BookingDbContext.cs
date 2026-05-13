using BookingService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Persistence
{
    /// <summary>
    /// Contesto EF Core per il servizio di prenotazioni.
    /// Gestisce la configurazione del modello e l'accesso al database.
    /// </summary>
    public class BookingDbContext : DbContext
    {
        /// <summary>
        /// Inizializza una nuova istanza di <see cref="BookingDbContext"/>.
        /// </summary>
        /// <param name="options">
        /// Opzioni di configurazione del contesto EF Core.
        /// </param>
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options)
        {
        }

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
                // Chiave primaria
                entity.HasKey(e => e.Id);

                // Campi obbligatori
                entity.Property(e => e.EmployeeId)
                    .IsRequired();

                entity.Property(e => e.CarId)
                    .IsRequired();

                entity.Property(e => e.StartTime)
                    .IsRequired();

                entity.Property(e => e.EndTime)
                    .IsRequired();

                // Stato della prenotazione (ACTIVE, COMPLETED, CANCELLED)
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                // Indici per ottimizzare le query più frequenti
                entity.HasIndex(e => e.EmployeeId);
                entity.HasIndex(e => e.CarId);
            });
        }
    }
}