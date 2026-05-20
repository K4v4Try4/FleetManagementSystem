using System.Runtime.Serialization;

namespace CarService.Shared.Enums
{
    /// <summary>
    /// Rappresenta lo stato operativo di un veicolo.
    /// </summary>
    /// <remarks>
    /// Questo enum è utilizzato per tracciare la disponibilità e l'utilizzo di un'auto.
    /// I valori sono serializzati come stringhe tramite l'attributo <see cref="EnumMemberAttribute"/>.
    /// </remarks>
    public enum CarStatus
    {
        /// <summary>
        /// Il veicolo è disponibile per l'uso.
        /// </summary>
        /// <remarks>
        /// Valore serializzato: "AVAILABLE".
        /// </remarks>
        [EnumMember(Value = "AVAILABLE")]
        AVAILABLE,

        /// <summary>
        /// Il veicolo è attualmente in uso.
        /// </summary>
        /// <remarks>
        /// Valore serializzato: "IN_USE".
        /// </remarks>
        [EnumMember(Value = "IN_USE")]
        IN_USE,

        /// <summary>
        /// Il veicolo è in manutenzione e non è disponibile.
        /// </summary>
        /// <remarks>
        /// Valore serializzato: "MAINTENANCE".
        /// </remarks>
        [EnumMember(Value = "MAINTENANCE")]
        MAINTENANCE
    }
}