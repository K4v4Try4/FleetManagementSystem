using BookingService.Business.Interfaces;
using BookingService.Repository.Interfaces;
using BookingService.Shared.DTOs;
using BookingService.Shared.Entities;
using BookingService.Shared.Enums;
using BookingService.Shared.Events;
using CarService.ClientHttp.Interfaces;
using EmployeeService.ClientHttp.Interfaces;

namespace BookingService.Business.Services
{
    public class BookingBusinessService : IBookingBusinessService
    {
        private readonly IBookingRepository _repository;
        private readonly ICarClient _carClient;
        private readonly IEmployeeClient _employeeClient;
        private readonly IBookingEventProducer _eventProducer;

        public BookingBusinessService(
            IBookingRepository repository,
            ICarClient carClient,
            IEmployeeClient employeeClient,
            IBookingEventProducer eventProducer)
        {
            _repository = repository;
            _carClient = carClient;
            _employeeClient = employeeClient;
            _eventProducer = eventProducer;
        }

        public async Task<int> CreateBookingAsync(CreateBookingDto dto)
        {
            // 1. Validazione Impiegato (Sincrona) 
            var isEligible = await _employeeClient.GetEmployeeEligibilityAsync(dto.EmployeeId);
            if (!isEligible) throw new InvalidOperationException("L'impiegato non è abilitato alla guida.");

            var alreadyHasBooking = await _repository.HasActiveBookingAsync(dto.EmployeeId);
            if (alreadyHasBooking) throw new InvalidOperationException("L'impiegato ha già una prenotazione attiva.");

            // 2. Validazione Auto (Sincrona) 
            var car = await _carClient.GetCarByIdAsync(dto.CarId);
            if (car == null || car.Status != "AVAILABLE") throw new InvalidOperationException("L'auto non è disponibile.");

            // 3. Creazione record locale
            var booking = new Booking
            {
                EmployeeId = dto.EmployeeId,
                CarId = dto.CarId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = BookingStatus.ACTIVE
            };

            await _repository.AddAsync(booking);
            await _repository.SaveChangesAsync();

            // 4. Side effect: Aggiorna stato auto a IN_USE 
            await _carClient.UpdateCarStatusAsync(dto.CarId, "IN_USE");

            return booking.Id;
        }

        public async Task ProcessCompletionAsync(short bookingId, double kmTraveled)
        {
            var booking = await _repository.GetByIdAsync(bookingId);
            if (booking == null || booking.Status != BookingStatus.ACTIVE) return;

            // Aggiorna stato locale
            booking.Status = BookingStatus.COMPLETED;
            await _repository.SaveChangesAsync();

            // Pubblica evento per il Car Service 
            await _eventProducer.PublishTripCompletedAsync(new TripCompletedEvent
            {
                VehicleId = booking.CarId,
                BookingId = booking.Id,
                KilometersTraveled = kmTraveled,
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(b => new BookingDto(b.Id, b.EmployeeId, b.CarId, b.StartTime, b.EndTime, b.Status.ToString()));
        }

        public async Task<BookingDto?> GetBookingByIdAsync(short id)
        {
            var b = await _repository.GetByIdAsync(id);
            return b == null ? null : new BookingDto(b.Id, b.EmployeeId, b.CarId, b.StartTime, b.EndTime, b.Status.ToString());
        }
    }
}