namespace BookingService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object utilizzato per la creazione
    /// di una nuova prenotazione.
    /// </summary>
    /// <param name="EmployeeId">
    /// Identificativo del dipendente che effettua la prenotazione.
    /// </param>
    /// <param name="CarId">
    /// Identificativo del veicolo da prenotare.
    /// </param>
    /// <param name="StartTime">
    /// Data e ora di inizio della prenotazione.
    /// </param>
    /// <param name="EndTime">
    /// Data e ora di fine della prenotazione.
    /// </param>
    public record CreateBookingDto(
        short EmployeeId,
        short CarId,
        DateTime StartTime,
        DateTime EndTime
    );
}