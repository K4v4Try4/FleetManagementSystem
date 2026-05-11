using BookingService.Repository.Interfaces;
using BookingService.Repository.Persistence;
using BookingService.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository.Implementations;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context) => _context = context;

    public async Task<Booking> GetByIdAsync(short id) => await _context.Bookings.FindAsync(id);

    public async Task<IEnumerable<Booking>> GetAllAsync() => await _context.Bookings.ToListAsync();

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasActiveBookingAsync(short employeeId)
    {
        return await _context.Bookings.AnyAsync(b => b.EmployeeId == employeeId && b.Status.Equals("ACTIVE"));
    }
}