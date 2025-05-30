using FluentAssertions;
using ProductMS.Application.Commands;
using ProductMS.Application.Validators;
using ProductMS.Commons.Dtos.Request;
using System.Linq;
using Xunit;

namespace ProductMS.Tests
{
    public class CreateProductValidatorTests
    {
        private readonly CreateProductValidator _validator;

        public CreateProductValidatorTests()
        {
            _validator = new CreateProductValidator();
        }

        [Fact]
        public void Validate_ValidCommand_ReturnsNoErrors()
        {
            // Arrange
            var dto = new ProductRequestDto(
                Name: "iPhone 15",
                Description: "Teléfono nuevo",
                Category: "Electrónica",
                BasePrice: 999.99m,
                ImageUrl: "https://example.com/image.jpg",
                SellerId: 123);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_EmptyName_ReturnsValidationError()
        {
            // Arrange
            var dto = new ProductRequestDto(
                Name: "",
                Description: "Teléfono nuevo",
                Category: "Electrónica",
                BasePrice: 999.99m,
                ImageUrl: "https://example.com/image.jpg",
                SellerId: 123);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Dto.Name" && e.ErrorMessage == "El nombre es requerido");
        }

        [Fact]
        public void Validate_LongName_ReturnsValidationError()
        {
            // Arrange
            var longName = new string('A', 101);
            var dto = new ProductRequestDto(
                Name: longName,
                Description: "Teléfono nuevo",
                Category: "Electrónica",
                BasePrice: 999.99m,
                ImageUrl: "https://example.com/image.jpg",
                SellerId: 123);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Dto.Name" && e.ErrorMessage == "El nombre no puede exceder 100 caracteres");
        }

        [Fact]
        public void Validate_NegativeBasePrice_ReturnsValidationError()
        {
            // Arrange
            var dto = new ProductRequestDto(
                Name: "iPhone 15",
                Description: "Teléfono nuevo",
                Category: "Electrónica",
                BasePrice: -100m,
                ImageUrl: "https://example.com/image.jpg",
                SellerId: 123);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Dto.BasePrice" && e.ErrorMessage == "El precio base debe ser mayor a 0");
        }

        [Fact]
        public void Validate_EmptyCategory_ReturnsValidationError()
        {
            // Arrange
            var dto = new ProductRequestDto(
                Name: "iPhone 15",
                Description: "Teléfono nuevo",
                Category: "",
                BasePrice: 999.99m,
                ImageUrl: "https://example.com/image.jpg",
                SellerId: 123);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Dto.Category" && e.ErrorMessage == "La categoría es requerida");
        }

        [Fact]
        public void Validate_LongCategory_ReturnsValidationError()
        {
            // Arrange
            var longCategory = new string('A', 51);
            var dto = new ProductRequestDto(
                Name: "iPhone 15",
                Description: "Teléfono nuevo",
                Category: longCategory,
                BasePrice: 999.99m,
                ImageUrl: "https://example.com/image.jpg",
                SellerId: 123);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Dto.Category" && e.ErrorMessage == "La categoría no puede exceder 50 caracteres");
        }

        [Fact]
        public void Validate_ZeroSellerId_ReturnsValidationError()
        {
            // Arrange
            var dto = new ProductRequestDto(
                Name: "iPhone 15",
                Description: "Teléfono nuevo",
                Category: "Electrónica",
                BasePrice: 999.99m,
                ImageUrl: "https://example.com/image.jpg",
                SellerId: 0);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Dto.SellerId" && e.ErrorMessage == "El ID del vendedor es requerido");
        }

        [Fact]
        public void Validate_EmptyImageUrl_ReturnsValidationError()
        {
            // Arrange
            var dto = new ProductRequestDto(
                Name: "iPhone 15",
                Description: "Teléfono nuevo",
                Category: "Electrónica",
                BasePrice: 999.99m,
                ImageUrl: "",
                SellerId: 123);
            var command = new CreateProductCommand(dto);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Dto.ImageUrl" && e.ErrorMessage == "La URL de la imagen es requerida");
        }
    }
}