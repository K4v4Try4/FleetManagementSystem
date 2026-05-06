using CarService.Business.Interfaces;
using CarService.Repository.Interfaces;
using CarService.Shared.DTOs;
using CarService.Shared.Entities;
using CarService.Shared.Enums;

namespace CarService.Business.Services
{
    /// <summary>
    /// Implementazione del servizio di business per la gestione dei veicoli.
    /// </summary>
    /// <remarks>
    /// Questo servizio contiene la logica applicativa principale del dominio CarService,
    /// orchestrando le operazioni tra repository e DTO e applicando le regole di business.
    /// </remarks>
    public class CarBusinessService : ICarBusinessService
    {
        private readonly ICarRepository _repository;

        /// <summary>
        /// Soglia di chilometraggio oltre la quale il veicolo richiede manutenzione.
        /// </summary>
        /// <remarks>
        /// Valore configurabile che determina il cambio di stato automatico a MAINTENANCE.
        /// </remarks>
        private const double MaintenanceThreshold = 15000;

        /// <summary>
        /// Inizializza una nuova istanza del servizio di business.
        /// </summary>
        /// <param name="repository">
        /// Repository utilizzato per l'accesso ai dati dei veicoli.
        /// </param>
        public CarBusinessService(ICarRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CarDto>> GetAvailableCarsAsync()
        {
            var cars = await _repository.GetAllAsync();

            return cars.Select(c => new CarDto(c.Id, c.PlateNumber, c.Model, c.Status.ToString(), c.Mileage));
        }

        /// <inheritdoc/>
        public async Task<CarDto?> GetCarByIdAsync(short id)
        {
            var c = await _repository.GetByIdAsync(id);

            return c == null ? null : new CarDto(c.Id, c.PlateNumber, c.Model, c.Status.ToString(), c.Mileage);
        }

        /// <inheritdoc/>
        public async Task<short> CreateCarAsync(CreateCarDto dto)
        {
            var car = new Car
            {
                // Non creo l'ID manualmente, lo lascio da fare al database
                PlateNumber = dto.PlateNumber,
                Model = dto.Model,
                Mileage = 0.0,
                Status = CarStatus.AVAILABLE
            };

            await _repository.AddAsync(car);
            await _repository.SaveChangesAsync();

            return car.Id;
        }

        /// <inheritdoc/>
        public async Task UpdateStatusAsync(short id, string status)
        {
            await _repository.UpdateStatusAsync(id, status);
            await _repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task ProcessTripCompletionAsync(short carId, double kmTraveled)
        {
            var car = await _repository.GetByIdAsync(carId);
            if (car == null) return;

            // Aggiornamento chilometraggio del veicolo
            car.Mileage += kmTraveled;

            // Regola di business: manutenzione automatica oltre soglia
            if (car.Mileage >= MaintenanceThreshold)
            {
                car.Status = CarStatus.MAINTENANCE;
            }
            else
            {
                car.Status = CarStatus.AVAILABLE;
            }

            await _repository.SaveChangesAsync();
        }
    }
}