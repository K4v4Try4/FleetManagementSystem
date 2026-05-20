namespace EmployeeService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object utilizzato per la creazione di un nuovo dipendente.
    /// </summary>
    /// <remarks>
    /// Questo DTO contiene solo le informazioni necessarie per registrare un dipendente nel sistema.
    /// L'identificativo viene gestito automaticamente dal sistema.
    /// </remarks>
    public record CreateEmployeeDto(
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