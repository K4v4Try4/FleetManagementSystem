using Confluent.Kafka;
using EmployeeService.Business.Interfaces;
using EmployeeService.Shared.Events;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace EmployeeService.Business.Kafka
{
    /// <summary>
    /// Implementazione del producer di eventi per Employee basato su Apache Kafka.
    /// Si occupa della serializzazione e pubblicazione degli eventi sul topic configurato.
    /// </summary>
    public class EmployeeEventProducer : IEmployeeEventProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        /// <summary>
        /// Inizializza il producer Kafka utilizzando la configurazione applicativa.
        /// </summary>
        /// <param name="configuration">Configurazione dell'applicazione (appsettings).</param>
        public EmployeeEventProducer(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";

            _topic = configuration["Kafka:EmployeeCreatedTopic"] ?? "employee-created-topic";

            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        /// <summary>
        /// Pubblica un evento di creazione Employee sul topic Kafka configurato.
        /// </summary>
        /// <param name="event">Evento EmployeeCreatedEvent da serializzare e inviare.</param>
        public async Task PublishEmployeeCreatedAsync(EmployeeCreatedEvent @event)
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