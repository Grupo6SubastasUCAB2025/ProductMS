using MassTransit;
using ProductMS.Infrastructure.EventBus.Events;
using MongoDB.Driver;

namespace ProductMS.Infrastructure.EventBus.Consumers
{
    // Consumidor del evento ProductCreated para guardar en MongoDB
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        // Colección de MongoDB para productos
        private readonly IMongoCollection<ProductCreated> _mongoCollection;

        // Constructor con inyección de dependencias
        public ProductCreatedConsumer(IMongoDatabase mongoDatabase)
        {
            _mongoCollection = mongoDatabase.GetCollection<ProductCreated>("Products");
        }

        // Procesa el evento y guarda los datos en MongoDB
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            await _mongoCollection.InsertOneAsync(context.Message);
        }
    }
}