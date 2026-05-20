namespace BookingService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object che rappresenta una prenotazione.
    /// </summary>
    /// <remarks>
    /// Questo DTO viene utilizzato per trasferire informazioni sulla prenotazione.
    /// </remarks>
    public record BookingDto(
        /// <summary>
        /// Identificativo univoco del veicolo.
        /// </summary>
        short Id,

        /// <summary>
        /// Identificativo del dipendente che ha effettuato la prenotazione.
        /// </summary>
        short EmployeeId,

        /// <summary>
        /// Identificativo del veicolo associato alla prenotazione.
        /// </summary>
        short CarId,

        /// <summary>
        /// Data e ora di inizio della prenotazione.
        /// </summary>
        DateTime StartTime,

        /// <summary>
        /// Data e ora di fine della prenotazione.
        /// </summary>
        DateTime EndTime,

        /// <summary>
        /// Stato corrente della prenotazione.
        /// </summary>
        string Status
    );
}