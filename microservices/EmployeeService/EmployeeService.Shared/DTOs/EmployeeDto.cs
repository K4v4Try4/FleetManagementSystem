namespace EmployeeService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object che rappresenta un dipendente.
    /// Usato per trasferire dati tra livelli dell'applicazione (API, service, client).
    /// </summary>
    /// <param name="Id">Identificativo univoco del dipendente.</param>
    /// <param name="Name">Nome completo del dipendente.</param>
    /// <param name="Role">Ruolo o posizione lavorativa del dipendente.</param>
    /// <param name="Email">Indirizzo email del dipendente.</param>
    /// <param name="Eligibility">Indica se il dipendente è abilitato/idoneo.</param>
    public record EmployeeDto(
        short Id,
        string Name,
        string Role,
        string Email,
        bool Eligibility
    );
}