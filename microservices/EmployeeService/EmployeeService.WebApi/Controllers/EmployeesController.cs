using EmployeeService.Business.Interfaces;
using EmployeeService.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.WebApi.Controllers
{
    /// <summary>
    /// Controller REST per la gestione delle operazioni sui dipendenti.
    /// </summary>
    /// <remarks>
    /// Espone endpoint HTTP per la gestione dei dipendenti (CRUD e aggiornamenti parziali).
    /// La logica applicativa è delegata al layer <see cref="IEmployeeBusinessService"/>.
    /// Include gestione delle eccezioni con logging e mapping verso codici HTTP appropriati.
    /// </remarks>
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeBusinessService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        /// <summary>
        /// Inizializza una nuova istanza del controller.
        /// </summary>
        /// <param name="employeeService">
        /// Servizio di business per la gestione delle operazioni sui dipendenti.
        /// </param>
        /// <param name="logger">
        /// Logger per il tracciamento degli errori e delle informazioni diagnostiche.
        /// </param>
        public EmployeesController(IEmployeeBusinessService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera l'elenco dei dipendenti disponibili.
        /// </summary>
        /// <returns>
        /// 200 OK con una collezione di <see cref="EmployeeDto"/> se l'operazione ha successo. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// GET /api/employees
        /// </remarks>
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
        /// Recupera un dipendente tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>
        /// 200 OK con il <see cref="EmployeeDto"/> se trovato. <br/>
        /// 404 Not Found se il dipendente non esiste. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// GET /api/employees/{id}
        /// </remarks>
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
        /// Verifica se un dipendente è abilitato alla guida.
        /// </summary>
        /// <param name="id">Identificativo del dipendente.</param>
        /// <returns>
        /// 200 OK con valore booleano che indica l'idoneità alla guida. <br/>
        /// 404 Not Found se il dipendente non esiste. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// GET /api/employees/{id}/eligibility
        /// </remarks>
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
        /// Crea un nuovo dipendente nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione del dipendente.</param>
        /// <returns>
        /// 201 Created con l'identificativo del dipendente creato. <br/>
        /// 400 Bad Request se i dati forniti non sono validi. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// POST /api/employees
        /// </remarks>
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