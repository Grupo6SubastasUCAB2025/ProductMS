using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductMS.Application.Commands;
using ProductMS.Application.Queries;
using ProductMS.Commons.Dtos.Request;
using ProductMS.Commons.Dtos.Response;

namespace ProductMS.Controllers
{
    // Controlador para manejar las solicitudes HTTP de productos
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // Mediador para delegar la lógica a los manejadores
        private readonly IMediator _mediator;

        // Constructor con inyección de dependencias
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Endpoint POST para crear un producto
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto dto)
        {
            // Crear el comando y delegar al manejador
            var command = new CreateProductCommand(dto);
            var response = await _mediator.Send(command);

            // Retornar respuesta con la ubicación del recurso creado
            return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
        }

        // Endpoint GET para obtener un producto por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
        {
            // Crear la consulta y delegar al manejador
            var query = new GetProductByIdQuery(id);
            var response = await _mediator.Send(query);

            // Retornar el DTO de respuesta
            return Ok(response);
        }
    }
}