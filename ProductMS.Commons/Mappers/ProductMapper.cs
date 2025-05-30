using ProductMS.Commons.Dtos.Request;
using ProductMS.Commons.Dtos.Response;
using ProductMS.Domain.Entities;

namespace ProductMS.Commons.Mappers
{
    // Clase estática para mapear entre entidades y DTOs
    public static class ProductMapper
    {
        // Convierte un DTO de solicitud a una entidad Product
        public static Product ToEntity(ProductRequestDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                BasePrice = dto.BasePrice,
                Images = dto.ImageUrl, // La URL se actualiza después con la de Firebase
                SellerId = dto.SellerId // Cambiado para usar int
            };
        }

        // Convierte una entidad Product a un DTO de respuesta
        public static ProductResponseDto ToDto(Product entity)
        {
            return new ProductResponseDto(
                entity.Id,
                entity.Name,
                entity.Description,
                entity.Category,
                entity.BasePrice,
                entity.Status,
                entity.Images,
                entity.SellerId, // Cambiado para usar int
                entity.CreatedAt
            );
        }
    }
}