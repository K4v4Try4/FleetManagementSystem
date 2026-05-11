using BookingService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Persistence;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options)
    {
    }

    // DbSet per la gestione delle prenotazioni
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurazione dell'entità Booking basata sulle specifiche
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.EmployeeId)
                .IsRequired();

            entity.Property(e => e.CarId)
                .IsRequired();

            entity.Property(e => e.StartTime)
                .IsRequired();

            entity.Property(e => e.EndTime)
                .IsRequired();

            // Lo stato può essere ACTIVE, COMPLETED o CANCELLED
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            // Indici per velocizzare le ricerche comuni
            entity.HasIndex(e => e.EmployeeId);
            entity.HasIndex(e => e.CarId);
        });
    }
}