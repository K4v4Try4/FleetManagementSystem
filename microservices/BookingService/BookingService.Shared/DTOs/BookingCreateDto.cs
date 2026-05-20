namespace BookingService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object utilizzato per la creazione di un nuova prenotazione.
    /// </summary>
    /// <remarks>
    /// Questo DTO contiene solo le informazioni necessarie per registrare una prenotazione nel sistema.
    /// Lo stato e l'identificativo vengono gestiti automaticamente dal sistema.
    /// </remarks>
    public record CreateBookingDto(
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
        DateTime EndTime
    );
}