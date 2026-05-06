using CarService.Shared.Enums;

namespace CarService.Shared.Entities
{
    /// <summary>
    /// Rappresenta un veicolo gestito dal sistema dal "Fleet Management System".
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Identificativo univoco del veicolo.
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// Numero di targa del veicolo.
        /// </summary>
        public required string PlateNumber { get; set; }

        /// <summary>
        /// Modello del veicolo.
        /// </summary>
        public required string Model { get; set; }

        /// <summary>
        /// Stato attuale del veicolo.
        /// </summary>
        /// <remarks>
        public required CarStatus Status { get; set; }

        /// <summary>
        /// Chilometraggio totale del veicolo.
        /// </summary>
        /// <remarks>
        /// Espresso in chilometri. Il valore predefinito è 0.0 e rappresenta un veicolo nuovo o senza percorrenza registrata.
        /// </remarks>
        public required double Mileage { get; set; } = 0.0;
    }
}