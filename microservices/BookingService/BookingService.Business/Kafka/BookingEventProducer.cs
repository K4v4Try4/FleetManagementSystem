using Confluent.Kafka;
using BookingService.Business.Interfaces;
using BookingService.Shared.Events;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BookingService.Business.Kafka
{
    public class BookingEventProducer : IBookingEventProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public BookingEventProducer(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
            _topic = configuration["Kafka:TripCompletedTopic"] ?? "trip-completed-topic";

            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishTripCompletedAsync(TripCompletedEvent @event)
        {
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(@event) };
            await _producer.ProduceAsync(_topic, message);
        }

        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}