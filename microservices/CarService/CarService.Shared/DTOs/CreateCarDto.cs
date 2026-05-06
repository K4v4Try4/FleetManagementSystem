namespace CarService.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object utilizzato per la creazione di un nuovo veicolo.
    /// </summary>
    /// <remarks>
    /// Questo DTO contiene solo le informazioni necessarie per registrare un'auto nel sistema.
    /// Lo stato e l'identificativo vengono gestiti automaticamente dal sistema.
    /// </remarks>
    public record CreateCarDto(
        /// <summary>
        /// Numero di targa del veicolo da creare.
        /// </summary>
        /// <remarks>
        /// Deve essere univoco nel sistema.
        /// </remarks>
        string PlateNumber,

        /// <summary>
        /// Modello del veicolo.
        /// </summary>
        string Model
    );
}