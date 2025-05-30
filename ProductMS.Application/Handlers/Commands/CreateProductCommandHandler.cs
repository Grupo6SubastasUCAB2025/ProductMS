using MediatR;
using ProductMS.Application.Commands;
using ProductMS.Commons.Dtos.Response;
using ProductMS.Commons.Mappers;
using ProductMS.Core.Persistence.Repositories.PostgreSQL;
using ProductMS.Core.EventBus;
using ProductMS.Core.Services;
using ProductMS.Infrastructure.EventBus.Events;
using System.Threading.Tasks;
using System.Threading;

namespace ProductMS.Application.Handlers.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventBus _eventBus;
        private readonly IImageStorageService _imageStorageService;

        public CreateProductCommandHandler(IProductRepository productRepository, IEventBus eventBus, IImageStorageService imageStorageService)
        {
            _productRepository = productRepository;
            _eventBus = eventBus;
            _imageStorageService = imageStorageService;
        }

        public async Task<ProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = ProductMapper.ToEntity(request.Dto);
            await _productRepository.AddAsync(product);
            var imageUrl = await _imageStorageService.UploadImageAsync(request.Dto.ImageUrl, product.Id.ToString());
            product.Images = imageUrl;
            await _productRepository.UpdateAsync(product);

            var productCreatedEvent = new ProductCreated
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                BasePrice = product.BasePrice,
                Images = product.Images,
                SellerId = product.SellerId,
                Status = product.Status,
                CreatedAt = product.CreatedAt
            };
            await _eventBus.PublishAsync(productCreatedEvent);

            return ProductMapper.ToDto(product);
        }
    }
}