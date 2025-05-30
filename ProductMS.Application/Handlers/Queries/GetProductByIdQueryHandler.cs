using MediatR;
using ProductMS.Application.Queries;
using ProductMS.Commons.Dtos.Response;
using ProductMS.Commons.Mappers;
using ProductMS.Core.Persistence.Repositories.PostgreSQL;

namespace ProductMS.Application.Handlers.Queries
{
    // Manejador para la consulta GetProductByIdQuery
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        // Repositorio para operaciones con la base de datos
        private readonly IProductRepository _productRepository;

        // Constructor con inyección de dependencias
        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Maneja la lógica para obtener un producto por su ID
        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Obtener el producto desde PostgreSQL
            var product = await _productRepository.GetByIdAsync(request.Id);

            // Verificar si el producto existe
            if (product == null)
            {
                throw new KeyNotFoundException($"Producto con ID {request.Id} no encontrado.");
            }

            // Mapear la entidad a un DTO de respuesta
            return ProductMapper.ToDto(product);
        }
    }
}