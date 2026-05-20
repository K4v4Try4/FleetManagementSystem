using CarService.Repository.Interfaces;
using CarService.Repository.Persistence;
using CarService.Shared.Entities;
using CarService.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarService.Repository.Implementations
{
    /// <summary>
    /// Implementazione del repository per la gestione delle entità <see cref="Car"/>.
    /// </summary>
    /// <remarks>
    /// Implementa le operazioni definite in <see cref="ICarRepository"/>.
    /// </remarks>
    public class CarRepository : ICarRepository
    {
        private readonly CarDbContext _context;

        /// <summary>
        /// Inizializza una nuova istanza del repository.
        /// </summary>
        /// <param name="context">
        /// Contesto EF Core utilizzato per l'accesso al database.
        /// </param>
        public CarRepository(CarDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Car>> GetAllAsync() => await _context.Cars.ToListAsync();

        /// <inheritdoc/>
        public async Task<Car?> GetByIdAsync(short id) => await _context.Cars.FindAsync(id);

        /// <inheritdoc/>
        public async Task AddAsync(Car car) => await _context.Cars.AddAsync(car);

        /// <inheritdoc/>
        public async Task UpdateStatusAsync(short id, string status)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null) car.Status = Enum.Parse<CarStatus>(status);
        }

        /// <inheritdoc/>
        public async Task UpdateMileageAsync(short id, double newMileage)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null) car.Mileage = newMileage;
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}