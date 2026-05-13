using BookingService.Shared.Events;

namespace BookingService.Business.Interfaces
{
    /// <summary>
    /// Definisce i metodi per la pubblicazione degli eventi relativi alle prenotazioni.
    /// </summary>
    public interface IBookingEventProducer
    {
        /// <summary>
        /// Pubblica un evento che segnala il completamento di un viaggio.
        /// </summary>
        /// <param name="event">
        /// Evento contenente le informazioni del viaggio completato.
        /// </param>
        /// <returns>
        /// Un task che rappresenta l'operazione asincrona di pubblicazione.
        /// </returns>
        Task PublishTripCompletedAsync(TripCompletedEvent @event);
    }
}