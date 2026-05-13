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
    /// <summary>
    /// Implementazione del servizio di business per la gestione delle prenotazioni.
    /// Coordina repository, servizi esterni e pubblicazione eventi.
    /// </summary>
    public class BookingBusinessService : IBookingBusinessService
    {
        private readonly IBookingRepository _repository;
        private readonly ICarClient _carClient;
        private readonly IEmployeeClient _employeeClient;
        private readonly IBookingEventProducer _eventProducer;

        /// <summary>
        /// Inizializza una nuova istanza di <see cref="BookingBusinessService"/>.
        /// </summary>
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

        /// <summary>
        /// Crea una nuova prenotazione dopo aver validato impiegato e veicolo.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione della prenotazione.</param>
        /// <returns>Identificativo della prenotazione creata.</returns>
        /// <exception cref="InvalidOperationException">
        /// Viene lanciata se l’impiegato non è idoneo o l’auto non è disponibile.
        /// </exception>
        public async Task<short> CreateBookingAsync(CreateBookingDto dto)
        {
            // 1. Validazione impiegato
            var isEligible = await _employeeClient.GetEmployeeEligibilityAsync(dto.EmployeeId);
            if (!isEligible)
                throw new InvalidOperationException("L'impiegato non è abilitato alla guida.");

            var alreadyHasBooking = await _repository.HasActiveBookingAsync(dto.EmployeeId);
            if (alreadyHasBooking)
                throw new InvalidOperationException("L'impiegato ha già una prenotazione attiva.");

            // 2. Validazione auto
            var car = await _carClient.GetCarByIdAsync(dto.CarId);
            if (car == null || car.Status != "AVAILABLE")
                throw new InvalidOperationException("L'auto non è disponibile.");

            // 3. Creazione prenotazione locale
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

            // 4. Side effect: aggiornamento stato veicolo
            await _carClient.UpdateCarStatusAsync(dto.CarId, "IN_USE");

            return booking.Id;
        }

        /// <summary>
        /// Completa una prenotazione e pubblica l’evento di viaggio completato.
        /// </summary>
        /// <param name="bookingId">Identificativo della prenotazione.</param>
        /// <param name="kmTraveled">Chilometri percorsi.</param>
        public async Task ProcessCompletionAsync(short bookingId, double kmTraveled)
        {
            var booking = await _repository.GetByIdAsync(bookingId);
            if (booking == null || booking.Status != BookingStatus.ACTIVE)
                return;

            // Aggiornamento stato locale
            booking.Status = BookingStatus.COMPLETED;
            await _repository.SaveChangesAsync();

            // Pubblicazione evento
            await _eventProducer.PublishTripCompletedAsync(new TripCompletedEvent
            {
                VehicleId = booking.CarId,
                BookingId = booking.Id,
                KilometersTraveled = kmTraveled,
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Recupera tutte le prenotazioni presenti nel sistema.
        /// </summary>
        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var list = await _repository.GetAllAsync();

            return list.Select(b =>
                new BookingDto(
                    b.Id,
                    b.EmployeeId,
                    b.CarId,
                    b.StartTime,
                    b.EndTime,
                    b.Status.ToString()));
        }

        /// <summary>
        /// Recupera una prenotazione tramite ID.
        /// </summary>
        public async Task<BookingDto?> GetBookingByIdAsync(short id)
        {
            var b = await _repository.GetByIdAsync(id);

            return b == null
                ? null
                : new BookingDto(
                    b.Id,
                    b.EmployeeId,
                    b.CarId,
                    b.StartTime,
                    b.EndTime,
                    b.Status.ToString());
        }
    }
}