namespace ProductMS.Commons.Dtos.Request
{
    // DTO para la solicitud de creación de un producto
    public record ProductRequestDto(
        // Nombre del producto
        string Name,
        // Descripción del producto
        string Description,
        // Categoría del producto
        string Category,
        // Precio base del producto
        decimal BasePrice,
        // URL temporal de la imagen proporcionada por el frontend
        string ImageUrl,
        // Identificador del vendedor
        int SellerId // Cambiado de Guid a int
    );
}