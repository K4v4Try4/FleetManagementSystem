namespace EmployeeService.Shared.Entities
{
    /// <summary>
    /// Rappresenta un dipendente gestito dal sistema del "Fleet Management System".
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Identificativo univoco del dipendente.
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// Nome del dipendente.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Posizione lavorativa del dipendente.
        /// </summary>
        public required string Role { get; set; }

        /// <summary>
        /// Indirizzo email del dipendente.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Indica se il dipendente è abilitato alla guida.
        /// </summary>
        public required bool Eligibility { get; set; }
    }
}