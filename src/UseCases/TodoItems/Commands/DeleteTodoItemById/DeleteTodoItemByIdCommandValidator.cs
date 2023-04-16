namespace MicroTodo.UseCases.TodoItems.Commands;

using FluentValidation;

public class DeleteTodoItemByIdCommandValidator : AbstractValidator<DeleteTodoItemByIdCommand>
{
    public DeleteTodoItemByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
