namespace ProductMS.Infrastructure.EventBus.Events
{
    public record ProductCreated
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public decimal BasePrice { get; init; }
        public string Images { get; init; } = string.Empty; // Agregado
        public int SellerId { get; init; }
        public string Status { get; init; } = "disponible";
        public DateTime CreatedAt { get; init; }
    }
}