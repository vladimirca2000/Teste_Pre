using FluentValidation;
using MeuTeste.Domain.DTOs;

namespace MeuTeste.Application.Validators
{
    public class ProductInputDtoValidator : AbstractValidator<ProductInputDto>
    {
        public ProductInputDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O nome do produto deve ter no mínimo 3 caracteres.")
                .MaximumLength(200)
                .WithMessage("O nome do produto não deve ultrapassar 200 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage("O preço deve ser maior que zero.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0)
                .WithMessage("Selecione uma categoria válida.");
        }
    }
}
