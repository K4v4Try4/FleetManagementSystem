using Confluent.Kafka;
using BookingService.Business.Interfaces;
using BookingService.Shared.Events;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BookingService.Business.Kafka
{
    /// <summary>
    /// Implementazione del producer Kafka responsabile della pubblicazione
    /// degli eventi relativi alle prenotazioni.
    /// </summary>
    public class BookingEventProducer : IBookingEventProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="BookingEventProducer"/>.
        /// </summary>
        /// <param name="configuration">
        /// Configurazione dell'applicazione contenente i parametri Kafka.
        /// </param>
        public BookingEventProducer(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
            _topic = configuration["Kafka:TripCompletedTopic"] ?? "trip-completed-topic";

            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        /// <summary>
        /// Pubblica un evento Kafka che segnala il completamento di un viaggio.
        /// </summary>
        /// <param name="event">
        /// Evento contenente le informazioni del viaggio completato.
        /// </param>
        /// <returns>
        /// Un task che rappresenta l'operazione asincrona di pubblicazione.
        /// </returns>
        public async Task PublishTripCompletedAsync(TripCompletedEvent @event)
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(@event)
            };

            await _producer.ProduceAsync(_topic, message);
        }

        /// <summary>
        /// Rilascia le risorse utilizzate dal producer Kafka.
        /// </summary>
        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}