using FluentValidation;
using MeuTeste.Domain.DTOs;

namespace MeuTeste.Application.Validators
{
    public class CategoryInputDtoValidator : AbstractValidator<CategoryInputDto>
    {
        public CategoryInputDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("O nome da categoria é obrigatório.")
                .MinimumLength(3)
                .WithMessage("O nome da categoria deve ter no mínimo 3 caracteres.")
                .MaximumLength(100)
                .WithMessage("O nome da categoria não deve ultrapassar 100 caracteres.");
        }
    }
}
