using BookingService.Repository.Interfaces;
using BookingService.Repository.Persistence;
using BookingService.Shared.Entities;
using BookingService.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementations
{
    /// <summary>
    /// Implementazione del repository per la gestione delle entità <see cref="Booking"/>.
    /// Fornisce operazioni CRUD e query specifiche sul database tramite EF Core.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _context;

        /// <summary>
        /// Inizializza una nuova istanza di <see cref="BookingRepository"/>.
        /// </summary>
        /// <param name="context">
        /// Contesto EF Core utilizzato per l'accesso al database.
        /// </param>
        public BookingRepository(BookingDbContext context) => _context = context;

        /// <summary>
        /// Recupera una prenotazione tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo della prenotazione.</param>
        /// <returns>
        /// L'entità <see cref="Booking"/> se trovata; altrimenti <c>null</c>.
        /// </returns>
        public async Task<Booking> GetByIdAsync(short id) =>
            await _context.Bookings.FindAsync(id);

        /// <summary>
        /// Recupera tutte le prenotazioni presenti nel database.
        /// </summary>
        /// <returns>Collezione di <see cref="Booking"/>.</returns>
        public async Task<IEnumerable<Booking>> GetAllAsync() =>
            await _context.Bookings.ToListAsync();

        /// <summary>
        /// Aggiunge una nuova prenotazione al database e salva le modifiche.
        /// </summary>
        /// <param name="booking">Entità da inserire.</param>
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Aggiorna una prenotazione esistente e persiste le modifiche.
        /// </summary>
        /// <param name="booking">Entità da aggiornare.</param>
        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Verifica se un impiegato ha già una prenotazione attiva.
        /// </summary>
        /// <param name="employeeId">Identificativo dell'impiegato.</param>
        /// <returns>
        /// <c>true</c> se esiste una prenotazione attiva; altrimenti <c>false</c>.
        /// </returns>
        public async Task<bool> HasActiveBookingAsync(short employeeId)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.EmployeeId == employeeId &&
                b.Status == BookingStatus.ACTIVE);
        }

        /// <summary>
        /// Salva le modifiche pendenti nel contesto di persistenza.
        /// </summary>
        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}