using System.Runtime.Serialization;

namespace BookingService.Shared.Enums
{
    /// <summary>
    /// Definisce i possibili stati di una prenotazione
    /// durante il suo ciclo di vita.
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Indica che la prenotazione è attiva,
        /// in corso oppure programmata per il futuro.
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        /// <summary>
        /// Indica che il viaggio associato alla prenotazione
        /// è terminato correttamente.
        /// </summary>
        [EnumMember(Value = "COMPLETED")]
        COMPLETED,

        /// <summary>
        /// Indica che la prenotazione è stata annullata.
        /// </summary>
        [EnumMember(Value = "CANCELLED")]
        CANCELLED
    }
}