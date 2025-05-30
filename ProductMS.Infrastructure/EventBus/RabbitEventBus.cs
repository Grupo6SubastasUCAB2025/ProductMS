using MassTransit;
using ProductMS.Core.EventBus;

namespace ProductMS.Infrastructure.EventBus
{
    // Implementación del bus de eventos usando RabbitMQ
    public class RabbitEventBus : IEventBus
    {
        // Endpoint para publicar eventos
        private readonly IPublishEndpoint _publishEndpoint;

        // Constructor con inyección de dependencias
        public RabbitEventBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        // Publica un evento a RabbitMQ
        public async Task PublishAsync<T>(T @event) where T : class
        {
            await _publishEndpoint.Publish(@event);
        }
    }
}