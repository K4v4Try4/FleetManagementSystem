using BookingService.Shared.Events;

namespace BookingService.Business.Interfaces
{
    public interface IBookingEventProducer
    {
        Task PublishTripCompletedAsync(TripCompletedEvent @event);
    }
}