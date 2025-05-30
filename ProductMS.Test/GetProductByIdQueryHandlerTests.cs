using FluentAssertions;
using Moq;
using ProductMS.Application.Handlers.Queries;
using ProductMS.Application.Queries;
using ProductMS.Commons.Dtos.Response;
using ProductMS.Commons.Mappers;
using ProductMS.Core.Persistence.Repositories.PostgreSQL;
using ProductMS.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductMS.Tests
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetProductByIdQueryHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_ReturnsProductResponseDto()
        {
            // Arrange
            var query = new GetProductByIdQuery(1);
            var product = new Product
            {
                Id = 1,
                Name = "iPhone 15",
                Description = "Teléfono nuevo",
                Category = "Electrónica",
                BasePrice = 999.99m,
                SellerId = 123,
                Images = "https://firebase.com/image.jpg",
                Status = "disponible",
                CreatedAt = DateTime.UtcNow
            };

            _productRepositoryMock.Setup(x => x.GetByIdAsync(query.Id))
                .ReturnsAsync(product);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("iPhone 15");
            result.Images.Should().Be("https://firebase.com/image.jpg");
            result.SellerId.Should().Be(123);
            _productRepositoryMock.Verify(x => x.GetByIdAsync(query.Id), Times.Once());
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsNull()
        {
            // Arrange
            var query = new GetProductByIdQuery(999);
            _productRepositoryMock.Setup(x => x.GetByIdAsync(query.Id))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _productRepositoryMock.Verify(x => x.GetByIdAsync(query.Id), Times.Once());
        }
    }
}