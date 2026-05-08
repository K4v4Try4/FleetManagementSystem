namespace EmployeeService.Shared.Events
{
    /// <summary>
    /// Evento che rappresenta la creazione di un nuovo dipendente.
    /// Viene tipicamente pubblicato su un message broker.
    /// </summary>
    public record EmployeeCreatedEvent
    {
        /// <summary>
        /// Identificativo del dipendente creato.
        /// </summary>
        public short EmployeeId { get; init; }

        /// <summary>
        /// Email del dipendente creato.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Indica se il dipendente è idoneo alla guida.
        /// </summary>
        public bool Eligibility { get; init; }

        /// <summary>
        /// Timestamp di creazione dell'evento in UTC.
        /// </summary>
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}