namespace ProductMS.Commons.Dtos.Response
{
    // DTO para la respuesta con los detalles del producto
    public record ProductResponseDto(
        // Identificador del producto
        int Id,
        // Nombre del producto
        string Name,
        // Descripción del producto
        string Description,
        // Categoría del producto
        string Category,
        // Precio base del producto
        decimal BasePrice,
        // Estado del producto
        string Status,
        // URL pública de la imagen en Firebase
        string Images,
        // Identificador del vendedor
        int SellerId, // Cambiado de Guid a int
        // Fecha de creación del producto
        DateTime CreatedAt
    );
}