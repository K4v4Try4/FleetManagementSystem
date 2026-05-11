namespace CarService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object che rappresenta un veicolo.
    /// </summary>
    /// <remarks>
    /// Questo DTO viene utilizzato per trasferire informazioni sull'auto
    /// tra i vari livelli dell'applicazione (API, servizi, client).
    /// </remarks>
    public record CarDto(
        /// <summary>
        /// Identificativo univoco del veicolo.
        /// </summary>
        short Id,

        /// <summary>
        /// Numero di targa del veicolo.
        /// </summary>
        string PlateNumber,

        /// <summary>
        /// Modello del veicolo.
        /// </summary>
        string Model,

        /// <summary>
        /// Stato attuale del veicolo.
        /// </summary>
        string Status,

        /// <summary>
        /// Chilometraggio totale del veicolo espresso in chilometri.
        /// </summary>
        double Mileage
    );
}