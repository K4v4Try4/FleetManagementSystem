using Confluent.Kafka;
using BookingService.Business.Interfaces;
using BookingService.Shared.Events;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BookingService.Business.Kafka
{
    /// <summary>
    /// Implementazione del producer di eventi per Booking basato su Apache Kafka.
    /// Si occupa della serializzazione e pubblicazione degli eventi sul topic configurato.
    /// </summary>
    public class BookingEventProducer : IBookingEventProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        /// <summary>
        /// Inizializza il producer Kafka utilizzando la configurazione applicativa.
        /// </summary>
        /// <param name="configuration">Configurazione dell'applicazione (appsettings).</param>
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
        public async Task PublishTripCompletedAsync(TripCompletedEvent @event)
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(@event)
            };

            await _producer.ProduceAsync(_topic, message);
        }

        /// <summary>
        /// Rilascia le risorse del producer Kafka e svuota il buffer dei messaggi.
        /// </summary>
        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}