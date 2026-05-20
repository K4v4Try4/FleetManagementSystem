using System.Runtime.Serialization;

namespace BookingService.Shared.Enums
{
    /// <summary>
    /// Definisce i possibili stati di una prenotazione.
    /// </summary>
    /// <remarks>
    /// Questo enum è utilizzato per tracciare lo stato di una prenotazione.
    /// I valori sono serializzati come stringhe tramite l'attributo <see cref="EnumMemberAttribute"/>.
    /// </remarks>
    public enum BookingStatus
    {
        /// <summary>
        /// Indica che la prenotazione è attiva.
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