using ProductMS.Commons.Dtos.Response;
using ProductMS.Commons.Dtos.Request;
using MediatR;

namespace ProductMS.Application.Commands
{
    // Comando para crear un producto, siguiendo el patrón CQRS
    public record CreateProductCommand(ProductRequestDto Dto) : IRequest<ProductResponseDto>;
}