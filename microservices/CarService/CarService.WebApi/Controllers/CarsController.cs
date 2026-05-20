using CarService.Business.Interfaces;
using CarService.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CarService.WebApi.Controllers
{
    /// <summary>
    /// Controller REST per la gestione delle operazioni sui veicoli.
    /// </summary>
    /// <remarks>
    /// Espone endpoint HTTP per la gestione delle auto (CRUD e aggiornamenti parziali).
    /// La logica applicativa è delegata al layer <see cref="ICarBusinessService"/>.
    /// Include gestione delle eccezioni con logging e mapping verso codici HTTP appropriati.
    /// </remarks>
    [ApiController]
    [Route("api/cars")]
    public class CarsController : ControllerBase
    {
        private readonly ICarBusinessService _carService;
        private readonly ILogger<CarsController> _logger;

        /// <summary>
        /// Inizializza una nuova istanza del controller.
        /// </summary>
        /// <param name="carService">
        /// Servizio di business per la gestione delle operazioni sui veicoli.
        /// </param>
        /// <param name="logger">
        /// Logger per il tracciamento degli errori e delle informazioni diagnostiche.
        /// </param>
        public CarsController(ICarBusinessService carService, ILogger<CarsController> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera l'elenco delle auto disponibili.
        /// </summary>
        /// <returns>
        /// 200 OK con una collezione di <see cref="CarDto"/> se l'operazione ha successo. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// GET /api/cars
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
        {
            try
            {
                var cars = await _carService.GetAvailableCarsAsync();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero delle auto");
                return StatusCode(500, "Errore interno del server");
            }
        }

        /// <summary>
        /// Recupera un veicolo tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo del veicolo.</param>
        /// <returns>
        /// 200 OK con il <see cref="CarDto"/> se trovato. <br/>
        /// 404 Not Found se il veicolo non esiste. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// GET /api/cars/{id}
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(short id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);

                if (car == null)
                    return NotFound();

                return Ok(car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero dell'auto con id {id}");
                return StatusCode(500, "Errore interno del server");
            }
        }

        /// <summary>
        /// Crea un nuovo veicolo nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione del veicolo.</param>
        /// <returns>
        /// 201 Created con l'identificativo del veicolo creato. <br/>
        /// 400 Bad Request se i dati forniti non sono validi. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// POST /api/cars
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<short>> CreateCar(CreateCarDto dto)
        {
            try
            {
                var id = await _carService.CreateCarAsync(dto);
                return CreatedAtAction(nameof(GetCar), new { id }, id);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Dati non validi per la creazione dell'auto");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione dell'auto");
                return StatusCode(500, "Errore interno del server");
            }
        }

        /// <summary>
        /// Aggiorna lo stato di un veicolo.
        /// </summary>
        /// <param name="id">Identificativo del veicolo.</param>
        /// <param name="status">Nuovo stato del veicolo.</param>
        /// <returns>
        /// 204 No Content se l'aggiornamento ha successo. <br/>
        /// 404 Not Found se il veicolo non esiste. <br/>
        /// 400 Bad Request se lo stato fornito non è valido. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// PATCH /api/cars/{id}/status
        /// </remarks>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(short id, [FromBody] string status)
        {
            try
            {
                await _carService.UpdateStatusAsync(id, status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Auto con id {id} non trovata");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Stato non valido");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante l'aggiornamento dello stato per id {id}");
                return StatusCode(500, "Errore interno del server");
            }
        }
    }
}