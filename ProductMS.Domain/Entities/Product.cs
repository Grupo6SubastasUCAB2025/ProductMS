namespace ProductMS.Domain.Entities
{
    // Added the missing "State" property to the Product class.  
    public class Product
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal BasePrice { get; set; }
        public string Status { get; set; }
        public string Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public string State { get; set; }

        // Constructor to initialize default values
        public Product()
        {
            State = "available";
            CreatedAt = DateTime.UtcNow;
        }
    }
}