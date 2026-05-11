namespace BookingService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object utilizzato per rappresentare
    /// le informazioni di una prenotazione.
    /// </summary>
    /// <param name="Id">
    /// Identificativo univoco della prenotazione.
    /// </param>
    /// <param name="EmployeeId">
    /// Identificativo del dipendente che ha effettuato la prenotazione.
    /// </param>
    /// <param name="CarId">
    /// Identificativo del veicolo associato alla prenotazione.
    /// </param>
    /// <param name="StartTime">
    /// Data e ora di inizio della prenotazione.
    /// </param>
    /// <param name="EndTime">
    /// Data e ora di fine della prenotazione.
    /// </param>
    /// <param name="Status">
    /// Stato corrente della prenotazione.
    /// </param>
    public record BookingDto(
        short Id,
        short EmployeeId,
        short CarId,
        DateTime StartTime,
        DateTime EndTime,
        string Status
    );
}