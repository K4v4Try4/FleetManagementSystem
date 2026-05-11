using BookingService.Shared.Enums;

namespace BookingService.Shared.Entities
{
    /// <summary>
    /// Rappresenta una prenotazione effettuata da un dipendente per un veicolo.
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// Identificativo univoco della prenotazione.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificativo del dipendente che ha effettuato la prenotazione.
        /// </summary>
        public short EmployeeId { get; set; }

        /// <summary>
        /// Identificativo del veicolo associato alla prenotazione.
        /// </summary>
        public short CarId { get; set; }

        /// <summary>
        /// Data e ora di inizio della prenotazione.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Data e ora di fine della prenotazione.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Stato corrente della prenotazione.
        /// </summary>
        public BookingStatus Status { get; set; }
    }
}