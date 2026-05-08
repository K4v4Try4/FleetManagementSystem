using EmployeeService.Business.Interfaces;
using EmployeeService.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.WebApi.Controllers
{
    /// <summary>
    /// Controller REST per la gestione degli impiegati.
    /// Espone gli endpoint per CRUD e verifica abilitazione guida.
    /// </summary>
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeBusinessService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeBusinessService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera la lista di tutti gli impiegati.
        /// GET /api/employees
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero impiegati");
                return StatusCode(500, "Errore interno del server");
            }
        }

        /// <summary>
        /// Recupera un singolo impiegato tramite ID.
        /// GET /api/employees/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(short id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null) return NotFound();

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nel recupero impiegato {id}");
                return StatusCode(500, "Errore interno");
            }
        }

        /// <summary>
        /// Endpoint cruciale per il Booking Service: verifica se l'impiegato può guidare.
        /// GET /api/employees/{id}/eligibility
        /// </summary>
        [HttpGet("{id}/eligibility")]
        public async Task<ActionResult<bool>> GetEligibility(short id)
        {
            try
            {
                var isEligible = await _employeeService.IsEmployeeEligibleForDrivingAsync(id);
                return Ok(isEligible);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore verifica eligibility per impiegato {id}");
                return StatusCode(500, "Errore interno");
            }
        }

        /// <summary>
        /// Crea un nuovo impiegato e scatena l'evento su Kafka.
        /// POST /api/employees
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<short>> Create(CreateEmployeeDto dto)
        {
            try
            {
                var id = await _employeeService.CreateEmployeeAsync(dto);
                // Ritorna 201 Created con il link per recuperare la risorsa appena creata
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione dell'impiegato");
                return BadRequest("Impossibile creare l'impiegato");
            }
        }
    }
}