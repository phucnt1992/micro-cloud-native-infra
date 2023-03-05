using FluentValidation;

namespace MicroTodo.UseCases.Commands;

public class UpdateTodoGroupCommandValidator : AbstractValidator<UpdateTodoGroupCommand>
{
    public UpdateTodoGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
