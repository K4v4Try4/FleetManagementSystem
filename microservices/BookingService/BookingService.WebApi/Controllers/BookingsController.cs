using BookingService.Business.Interfaces;
using BookingService.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.WebApi.Controllers
{
    /// <summary>
    /// Controller per la gestione del ciclo di vita delle prenotazioni.
    /// Coordina le interazioni tra EmployeeService e CarService.
    /// </summary>
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingBusinessService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingBusinessService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera tutte le prenotazioni a sistema.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        /// <summary>
        /// Recupera i dettagli di una specifica prenotazione.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetById(short id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        /// <summary>
        /// Crea una nuova prenotazione. Valida l'idoneità del dipendente 
        /// e la disponibilità dell'auto tramite chiamate HTTP sincrone.
        /// </summary>
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
        /// Endpoint per completare un viaggio. 
        /// Scatena l'evento asincrono TripCompleted per aggiornare il chilometraggio dell'auto.
        /// </summary>
        /// <param name="id">ID della prenotazione</param>
        /// <param name="kmTraveled">Chilometri percorsi durante il viaggio</param>
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