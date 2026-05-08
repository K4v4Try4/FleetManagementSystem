namespace EmployeeService.Shared.Entities
{
    /// <summary>
    /// Rappresenta un dipendente all'interno del sistema di "Employee Service".
    /// Contiene informazioni anagrafiche e di ruolo.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Identificativo univoco del dipendente.
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// Nome completo del dipendente.
        /// Campo obbligatorio.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Ruolo o posizione lavorativa del dipendente.
        /// Campo obbligatorio.
        /// </summary>
        public required string Role { get; set; }

        /// <summary>
        /// Indirizzo email del dipendente.
        /// Campo obbligatorio.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Indica se il dipendente è idoneo/abilitato a determinate attività.
        /// </summary>
        public required bool Eligibility { get; set; }
    }
}