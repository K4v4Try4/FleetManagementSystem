namespace EmployeeService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object che rappresenta un dipendente.
    /// </summary>
    /// <remarks>
    /// Questo DTO viene utilizzato per trasferire informazioni sull'auto.
    /// </remarks>
    public record EmployeeDto(
        /// <summary>
        /// Identificativo univoco del dipendente.
        /// </summary>
        short Id,

        /// <summary>
        /// Nome del dipendente.
        /// </summary>
        string Name,

        /// <summary>
        /// Posizione lavorativa del dipendente.
        /// </summary>
        string Role,

        /// <summary>
        /// Indirizzo email del dipendente.
        /// </summary>
        string Email,

        /// <summary>
        /// Indica se il dipendente è abilitato alla guida.
        /// </summary>
        bool Eligibility
    );
}