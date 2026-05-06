namespace CarService.Shared.Events
{
    /// <summary>
    /// Evento che rappresenta il completamento di un viaggio.
    /// </summary>
    /// <remarks>
    /// Questo evento viene pubblicato quando una corsa termina con successo
    /// e contiene le informazioni necessarie per aggiornare lo stato del veicolo
    /// </remarks>
    public record TripCompletedEvent
    {
        /// <summary>
        /// Identificativo del veicolo coinvolto nel viaggio.
        /// </summary>
        public short VehicleId { get; init; }

        /// <summary>
        /// Identificativo della prenotazione associata al viaggio.
        /// </summary>
        public short BookingId { get; init; }

        /// <summary>
        /// Distanza percorsa durante il viaggio, espressa in chilometri.
        /// </summary>
        public double KilometersTraveled { get; init; }

        /// <summary>
        /// Data e ora in cui il viaggio è stato completato.
        /// </summary>
        public DateTime Timestamp { get; init; }
    }
}