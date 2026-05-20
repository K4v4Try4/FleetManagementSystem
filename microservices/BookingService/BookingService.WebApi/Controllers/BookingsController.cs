using BookingService.Business.Interfaces;
using BookingService.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.WebApi.Controllers
{
    /// <summary>
    /// Controller REST per la gestione delle operazioni sulle prenotazioni.
    /// </summary>
    /// <remarks>
    /// Espone endpoint HTTP per la gestione delle prenotazioni (CRUD e aggiornamenti parziali).
    /// La logica applicativa è delegata al layer <see cref="IBookingBusinessService"/>.
    /// Include gestione delle eccezioni con logging e mapping verso codici HTTP appropriati.
    /// </remarks>
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingBusinessService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        /// <summary>
        /// Inizializza una nuova istanza del controller.
        /// </summary>
        /// <param name="employeeService">
        /// Servizio di business per la gestione delle operazioni sui dipendenti.
        /// </param>
        /// <param name="logger">
        /// Logger per il tracciamento degli errori e delle informazioni diagnostiche.
        /// </param>
        public BookingsController(IBookingBusinessService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera l'elenco delle prenotazioni disponibili.
        /// </summary>
        /// <returns>
        /// 200 OK con una collezione di <see cref="EmployeeDto"/> se l'operazione ha successo. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// GET /api/bookings
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }


        /// <summary>
        /// Recupera una prenotazione tramite identificativo.
        /// </summary>
        /// <param name="id">Identificativo della prenotazione.</param>
        /// <returns>
        /// 200 OK con il <see cref="EmployeeDto"/> se trovato. <br/>
        /// 404 Not Found se la prenotazione non esiste. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// GET /api/bookings/{id}
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetById(short id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        /// <summary>
        /// Crea una nuova prenotazione nel sistema.
        /// </summary>
        /// <param name="dto">Dati necessari alla creazione della prenotazione.</param>
        /// <returns>
        /// 201 Created con l'identificativo della prenotazione creata. <br/>
        /// 400 Bad Request se i dati forniti non sono validi. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// POST /api/bookings
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<short>> Create(CreateBookingDto dto)
        {
            try
            {
                var id = await _bookingService.CreateBookingAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validazione fallita per la prenotazione");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore imprevisto durante la creazione della prenotazione");
                return StatusCode(500, "Errore interno del server");
            }
        }

        /// <summary>
        /// Completa una prenotazione e registra i chilometri percorsi.
        /// </summary>
        /// <param name="id">Identificativo della prenotazione da completare.</param>
        /// <param name="kmTraveled">Chilometri percorsi durante il viaggio.</param>
        /// <returns>
        /// 204 No Content se l'operazione ha successo. <br/>
        /// 500 Internal Server Error in caso di errore imprevisto.
        /// </returns>
        /// <remarks>
        /// PATCH /api/bookings/{id}/status
        /// </remarks>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> CompleteBooking(short id, [FromBody] double kmTraveled)
        {
            try
            {
                await _bookingService.ProcessCompletionAsync(id, kmTraveled);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore nel completamento della prenotazione {id}");
                return StatusCode(500, "Errore durante l'aggiornamento dello stato");
            }
        }
    }
}