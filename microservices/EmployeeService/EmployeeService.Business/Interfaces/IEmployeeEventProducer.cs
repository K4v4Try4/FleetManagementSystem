using EmployeeService.Shared.Events;

namespace EmployeeService.Business.Interfaces
{
    /// <summary>
    /// Interfaccia che definisce un produttore di eventi per il dominio Employee.
    /// </summary>
    /// <remarks>
    /// Responsabile della pubblicazione di eventi su un sistema di messaging.
    /// </remarks>
    public interface IEmployeeEventProducer
    {
        /// <summary>
        /// Pubblica un evento di creazione di un Employee.
        /// </summary>
        /// <param name="event">Evento contenente i dati del dipendente creato.</param>
        Task PublishEmployeeCreatedAsync(EmployeeCreatedEvent @event);
    }
}