namespace MicroTodo.UseCases.TodoGroups.Commands;

using FluentValidation;

public class DeleteTodoGroupByIdCommandValidator : AbstractValidator<DeleteTodoGroupByIdCommand>
{
    public DeleteTodoGroupByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
