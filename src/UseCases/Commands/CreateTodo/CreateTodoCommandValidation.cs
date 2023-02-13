using FluentValidation;

namespace MicroTodo.UseCases.Commands;

public class CreateTodoCommandValidation : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidation()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
    }
}
