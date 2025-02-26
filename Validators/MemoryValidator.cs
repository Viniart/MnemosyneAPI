using FluentValidation;
using MnemosyneAPI.Model;

namespace MnemosyneAPI.Validators
{
    public class MemoryValidator : AbstractValidator<Memory>
    {
        public MemoryValidator()
        {
            RuleFor(x => x.Title)
                .NotNull().WithMessage("Título é obrigatório")
                .Length(3, 100).WithMessage("O Título deve ter de 3 a 100 caracteres");

            RuleFor(x => x.Description)
                .NotNull().WithMessage("Descrição é obrigatória")
                .NotEmpty().WithMessage("Descrição não pode ser vazio");

            RuleFor(x => x.Images)
                .NotNull().WithMessage("Imagem é obrigatória")
                .NotEmpty().WithMessage("Imagem não pode ser vazio");
        }
    }
}