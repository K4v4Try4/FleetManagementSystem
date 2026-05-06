using CarService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.Repository.Persistence
{
    /// <summary>
    /// Contesto Entity Framework Core per la gestione delle entità del dominio di "CarSharing".
    /// </summary>
    /// <remarks>
    /// Questo DbContext gestisce la connessione al database e la configurazione
    /// del modello dati relativo alle entità del servizio.
    /// </remarks>
    public class CarDbContext : DbContext
    {
        /// <summary>
        /// Inizializza una nuova istanza del contesto con le opzioni specificate.
        /// </summary>
        /// <param name="options">
        /// Opzioni di configurazione del DbContext (connection string, provider, ecc.).
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
        /// <remarks>
        /// In questo metodo vengono definite:
        /// - chiave primaria dell'entità Car
        /// - vincoli sulle proprietà (required, lunghezze massime)
        /// - conversione dell'enum CarStatus in stringa nel database
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurazione della tabella Car
            modelBuilder.Entity<Car>(entity =>
            {
                /// <summary>
                /// Definisce la chiave primaria dell'entità Car.
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
                /// <remarks>
                /// Permette maggiore leggibilità dei dati rispetto al valore numerico dell'enum.
                /// </remarks>
                entity.Property(e => e.Status).IsRequired().HasConversion<string>();

                /// <summary>
                /// Configura la proprietà <c>Mileage</c> dell'entità Car.
                /// </summary>
                /// <remarks>
                /// La proprietà è obbligatoria e rappresenta il chilometraggio del veicolo.
                /// </remarks>
                entity.Property(e => e.Mileage).IsRequired();
            });
        }
    }
}