using BookingService.Repository.Interfaces;
using BookingService.Repository.Persistence;
using BookingService.Shared.Entities;
using BookingService.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementations
{
    /// <summary>
    /// Implementazione del repository per la gestione delle entità <see cref="Booking"/>.
    /// </summary>
    /// <remarks>
    /// Implementa le operazioni definite in <see cref="IBookingRepository"/>.
    /// </remarks>
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _context;

        /// <summary>
        /// Inizializza una nuova istanza del repository.
        /// </summary>
        /// <param name="context">
        /// Contesto EF Core utilizzato per l'accesso al database.
        /// </param>
        public BookingRepository(BookingDbContext context) => _context = context;

        /// <inheritdoc/>
        public async Task<Booking> GetByIdAsync(short id) => await _context.Bookings.FindAsync(id);

        /// <inheritdoc/>
        public async Task<IEnumerable<Booking>> GetAllAsync() => await _context.Bookings.ToListAsync();

        /// <inheritdoc/>
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> HasActiveBookingAsync(short employeeId)
        {
            return await _context.Bookings.AnyAsync(b => b.EmployeeId == employeeId && b.Status == BookingStatus.ACTIVE);
        }

        /// <inheritdoc/>
        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}