namespace BookingService.Shared.Events
{
    /// <summary>
    /// Evento pubblicato quando una prenotazione viene completata.
    /// Viene utilizzato dal servizio veicoli per aggiornare
    /// il chilometraggio del mezzo associato.
    /// </summary>
    public record TripCompletedEvent
    {
        /// <summary>
        /// Identificativo del veicolo associato al viaggio completato.
        /// </summary>
        public short VehicleId { get; init; }

        /// <summary>
        /// Identificativo della prenotazione completata.
        /// </summary>
        public short BookingId { get; init; }

        /// <summary>
        /// Numero di chilometri percorsi durante il viaggio.
        /// </summary>
        public double KilometersTraveled { get; init; }

        /// <summary>
        /// Data e ora di pubblicazione dell'evento.
        /// </summary>
        public DateTime Timestamp { get; init; }
    }
}