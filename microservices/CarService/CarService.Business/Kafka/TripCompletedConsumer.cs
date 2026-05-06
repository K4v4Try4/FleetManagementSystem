using Confluent.Kafka;
using CarService.Business.Interfaces;
using CarService.Shared.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace CarService.Business.Kafka
{
    /// <summary>
    /// Consumer Kafka che ascolta gli eventi di completamento viaggio.
    /// </summary>
    /// <remarks>
    /// Questo BackgroundService consuma messaggi dal topic Kafka relativo ai viaggi completati
    /// e delega la logica di business al <see cref="ICarBusinessService"/>.
    /// </remarks>
    public class TripCompletedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _topic;
        private readonly string _bootstrapServers;

        /// <summary>
        /// Inizializza il consumer Kafka con configurazione esterna.
        /// </summary>
        /// <param name="serviceProvider">
        /// Service provider utilizzato per creare scope DI durante l'elaborazione dei messaggi.
        /// </param>
        /// <param name="configuration">
        /// Configurazione applicativa (Kafka topic e bootstrap servers).
        /// </param>
        public TripCompletedConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _topic = configuration["Kafka:TripCompletedTopic"] ?? "trip-completed-topic";
            _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
        }

        /// <summary>
        /// Esegue il loop di consumo dei messaggi Kafka fino alla cancellazione del token.
        /// </summary>
        /// <param name="stoppingToken">
        /// Token utilizzato per interrompere correttamente il servizio in fase di shutdown.
        /// </param>
        /// <remarks>
        /// Il metodo:
        /// - si connette al broker Kafka
        /// - sottoscrive il topic configurato
        /// - deserializza gli eventi <see cref="TripCompletedEvent"/>
        /// - invoca la logica di business tramite <see cref="ICarBusinessService"/>
        /// </remarks>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "car-service-group",
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        if (consumeResult?.Message?.Value != null)
                        {
                            var kafkaEvent = JsonSerializer.Deserialize<TripCompletedEvent>(
                                consumeResult.Message.Value);

                            if (kafkaEvent != null)
                            {
                                using var scope = _serviceProvider.CreateScope();
                                var carBusinessService = scope.ServiceProvider.GetRequiredService<ICarBusinessService>();

                                await carBusinessService.ProcessTripCompletionAsync(
                                    kafkaEvent.VehicleId,
                                    kafkaEvent.KilometersTraveled);
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Kafka error: {ex.Error.Reason}");
                        await Task.Delay(2000, stoppingToken); // retry
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Shutdown graceful del consumer Kafka
                consumer.Close();
            }
        }
    }
}