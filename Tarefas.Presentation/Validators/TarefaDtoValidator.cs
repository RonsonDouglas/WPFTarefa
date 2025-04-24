using FluentValidation;
using Tarefas.Presentation.Dtos;

namespace Tarefas.Presentation.Validators
{
    public class TarefaDtoValidator : AbstractValidator<TarefaDto>
    {
        public TarefaDtoValidator()
        {
            RuleFor(t => t.Titulo)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres.");

            RuleFor(t => t.Descricao)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(t => !string.IsNullOrWhiteSpace(t.Descricao));

            RuleFor(t => t.Status)
                .IsInEnum().WithMessage("Status inválido.");

            RuleFor(t => t.DataConclusao)
                .Must((dto, dataConclusao) =>
                {
                    if (!dataConclusao.HasValue) return true;
                    return dataConclusao.Value >= dto.DataCriacao;
                })
                .WithMessage("A data de conclusão não pode ser anterior à data de criação.");
        }
    }
}
