using BookingService.Shared.DTOs;

namespace BookingService.Business.Interfaces
{
    public interface IBookingBusinessService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto?> GetBookingByIdAsync(short id);
        Task<int> CreateBookingAsync(CreateBookingDto dto);
        Task ProcessCompletionAsync(short bookingId, double kmTraveled);
    }
}