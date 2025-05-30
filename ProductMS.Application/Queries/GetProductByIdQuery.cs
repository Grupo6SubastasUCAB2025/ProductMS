using ProductMS.Commons.Dtos.Response;
using MediatR;

namespace ProductMS.Application.Queries
{
    // Consulta para obtener un producto por su ID
    public record GetProductByIdQuery(int Id) : IRequest<ProductResponseDto>;
}