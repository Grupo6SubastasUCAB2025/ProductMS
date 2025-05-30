using FluentValidation;
using ProductMS.Application.Commands;
using ProductMS.Commons.Dtos.Request;

namespace ProductMS.Application.Validators
{
    // Validador para el comando CreateProductCommand
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            // Validar que el nombre no esté vacío y no exceda 100 caracteres
            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            // Validar que el precio base sea mayor a 0
            RuleFor(x => x.Dto.BasePrice)
                .GreaterThan(0).WithMessage("El precio base debe ser mayor a 0");

            // Validar que la categoría no esté vacía y no exceda 50 caracteres
            RuleFor(x => x.Dto.Category)
                .NotEmpty().WithMessage("La categoría es requerida")
                .MaximumLength(50).WithMessage("La categoría no puede exceder 50 caracteres");

            // Validar que el ID del vendedor no esté vacío
            RuleFor(x => x.Dto.SellerId)
                .NotEmpty().WithMessage("El ID del vendedor es requerido");

            // Validar que la URL de la imagen no esté vacía
            RuleFor(x => x.Dto.ImageUrl)
                .NotEmpty().WithMessage("La URL de la imagen es requerida");
        }
    }
}