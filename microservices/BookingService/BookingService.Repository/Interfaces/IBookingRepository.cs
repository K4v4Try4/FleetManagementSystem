using BookingService.Shared.Entities;

namespace BookingService.Repository.Interfaces;

public interface IBookingRepository
{
    Task<Booking> GetByIdAsync(short id);
    Task<IEnumerable<Booking>> GetAllAsync();
    Task AddAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task<bool> HasActiveBookingAsync(short employeeId);
}