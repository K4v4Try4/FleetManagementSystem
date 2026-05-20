using EmployeeService.Business.Interfaces;
using EmployeeService.Repository.Interfaces;
using EmployeeService.Shared.DTOs;
using EmployeeService.Shared.Entities;
using EmployeeService.Shared.Events;

namespace EmployeeService.Business.Services
{
    /// <summary>
    /// Implementazione del servizio di business per la gestione dei dipendenti.
    /// </summary>
    /// <remarks>
    /// Questo servizio contiene la logica applicativa principale del dominio EmployeeService,
    /// orchestrando le operazioni tra repository e DTO e applicando le regole di business.
    /// </remarks>
    public class EmployeeBusinessService : IEmployeeBusinessService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IEmployeeEventProducer _eventProducer;

        /// <summary>
        /// Inizializza una nuova istanza del servizio business tramite dependency injection.
        /// </summary>
        /// <param name="repository">Repository per accesso ai dati Employee.</param>
        /// <param name="eventProducer">Producer per la pubblicazione eventi Kafka.</param>
        public EmployeeBusinessService(IEmployeeRepository repository, IEmployeeEventProducer eventProducer)
        {
            _repository = repository;
            _eventProducer = eventProducer;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _repository.GetAllAsync();

            return employees.Select(e =>
                new EmployeeDto(e.Id, e.Name, e.Role, e.Email, e.Eligibility));
        }

        /// <inheritdoc/>
        public async Task<EmployeeDto?> GetEmployeeByIdAsync(short id)
        {
            var e = await _repository.GetByIdAsync(id);

            return e == null
                ? null
                : new EmployeeDto(e.Id, e.Name, e.Role, e.Email, e.Eligibility);
        }

        /// <inheritdoc/>
        public async Task<short> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            // Mapping DTO -> Entity
            var employee = new Employee
            {
                Name = dto.Name,
                Role = dto.Role,
                Email = dto.Email,
                Eligibility = dto.Eligibility
            };

            // Persistenza su database
            await _repository.AddAsync(employee);
            await _repository.SaveChangesAsync();

            // Pubblicazione evento Kafka (side effect asincrono)
            await _eventProducer.PublishEmployeeCreatedAsync(
                new EmployeeCreatedEvent
                {
                    EmployeeId = employee.Id,
                    Email = employee.Email
                });

            return employee.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> IsEmployeeEligibleForDrivingAsync(short id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return employee != null && employee.Eligibility;
        }
    }
}