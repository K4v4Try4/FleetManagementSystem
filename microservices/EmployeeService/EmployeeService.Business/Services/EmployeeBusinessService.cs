using EmployeeService.Business.Interfaces;
using EmployeeService.Repository.Interfaces;
using EmployeeService.Shared.DTOs;
using EmployeeService.Shared.Entities;
using EmployeeService.Shared.Events;

namespace EmployeeService.Business.Services
{
    /// <summary>
    /// Implementazione del servizio di business per la gestione degli Employee.
    /// Coordina repository, mapping DTO e pubblicazione eventi su Kafka.
    /// </summary>
    public class EmployeeBusinessService : IEmployeeBusinessService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IEmployeeEventProducer _eventProducer;

        /// <summary>
        /// Inizializza una nuova istanza del servizio business tramite dependency injection.
        /// </summary>
        /// <param name="repository">Repository per accesso ai dati Employee.</param>
        /// <param name="eventProducer">Producer per la pubblicazione eventi Kafka.</param>
        public EmployeeBusinessService(
            IEmployeeRepository repository,
            IEmployeeEventProducer eventProducer)
        {
            _repository = repository;
            _eventProducer = eventProducer;
        }

        /// <summary>
        /// Recupera tutti i dipendenti e li mappa in DTO.
        /// </summary>
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _repository.GetAllAsync();

            return employees.Select(e =>
                new EmployeeDto(e.Id, e.Name, e.Role, e.Email, e.Eligibility));
        }

        /// <summary>
        /// Recupera un dipendente tramite ID e lo converte in DTO.
        /// </summary>
        public async Task<EmployeeDto?> GetEmployeeByIdAsync(short id)
        {
            var e = await _repository.GetByIdAsync(id);

            return e == null
                ? null
                : new EmployeeDto(e.Id, e.Name, e.Role, e.Email, e.Eligibility);
        }

        /// <summary>
        /// Crea un nuovo dipendente, lo salva nel database e pubblica un evento Kafka.
        /// </summary>
        /// <param name="dto">Dati del dipendente da creare.</param>
        /// <returns>Identificativo del dipendente creato.</returns>
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

        /// <summary>
        /// Regola di business: verifica se un dipendente è idoneo alla guida.
        /// </summary>
        public async Task<bool> IsEmployeeEligibleForDrivingAsync(short id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return employee != null && employee.Eligibility;
        }
    }
}