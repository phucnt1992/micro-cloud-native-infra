namespace MicroTodo.UseCases.Commands;

using FluentValidation;

public class CreateTodoGroupCommandValidator : AbstractValidator<CreateTodoGroupCommand>
{
    public CreateTodoGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
