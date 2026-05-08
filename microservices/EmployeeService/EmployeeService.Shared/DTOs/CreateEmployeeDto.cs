namespace EmployeeService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object utilizzato per la creazione di un nuovo dipendente.
    /// Contiene solo i dati necessari in input, senza campi generati dal sistema.
    /// </summary>
    /// <param name="Name">Nome completo del dipendente.</param>
    /// <param name="Role">Ruolo o posizione lavorativa del dipendente.</param>
    /// <param name="Email">Indirizzo email del dipendente.</param>
    public record CreateEmployeeDto(
        string Name,
        string Role,
        string Email,
        bool Eligibility
    );
}